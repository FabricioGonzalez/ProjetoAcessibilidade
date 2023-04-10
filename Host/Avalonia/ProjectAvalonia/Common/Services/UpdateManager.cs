using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Http;
using ProjectAvalonia.Common.Microservices;
using ProjectAvalonia.Common.Models;
using ProjectAvalonia.Logging;

namespace ProjectAvalonia.Common.Services;

public class UpdateManager : IDisposable
{
    private const byte MaxTries = 2;

    public UpdateManager(
        string dataDir
        , bool downloadNewVersion
        , IHttpClient httpClient
    )
    {
        InstallerDir = Path.Combine(path1: dataDir, path2: "Installer");
        HttpClient = httpClient;
        DownloadNewVersion = downloadNewVersion;
    }

    private string InstallerPath
    {
        get;
        set;
    } = "";

    public string InstallerDir
    {
        get;
    }

    public IHttpClient HttpClient
    {
        get;
    }

    ///<summary> Comes from config file. Decides Wasabi should download the new installer in the background or not.</summary>
    public bool DownloadNewVersion
    {
        get;
    }

    ///<summary> Install new version on shutdown or not.</summary>
    public bool DoUpdateOnClose
    {
        get;
        set;
    }

    private UpdateChecker? UpdateChecker
    {
        get;
        set;
    }

    private CancellationToken CancellationToken
    {
        get;
        set;
    }

    public void Dispose()
    {
        if (UpdateChecker is { } updateChecker)
        {
            updateChecker.UpdateStatusChanged -= UpdateChecker_UpdateStatusChangedAsync;
        }
    }

    private async void UpdateChecker_UpdateStatusChangedAsync(
        object? sender
        , UpdateStatus updateStatus
    )
    {
        var tries = 0;
        var updateAvailable = !updateStatus.ClientUpToDate /*|| !updateStatus.BackendCompatible*/;
        var targetVersion = updateStatus.ClientVersion;

        if (!updateAvailable)
        {
            // After updating Wasabi, remove old installer file.
            Cleanup();
            return;
        }

        if (DownloadNewVersion && !RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Linux))
        {
            do
            {
                tries++;
                try
                {
                    var (installerPath, newVersion) = await GetInstallerAsync(targetVersion: targetVersion)
                        .ConfigureAwait(continueOnCapturedContext: false);
                    InstallerPath = installerPath;
                    Logger.LogInfo(message: $"Version {newVersion} downloaded successfuly.");
                    updateStatus.IsReadyToInstall = true;
                    updateStatus.ClientVersion = newVersion;
                    break;
                }
                catch (OperationCanceledException ex)
                {
                    Logger.LogTrace(message: "Getting new update was canceled.", exception: ex);
                    break;
                }
                catch (InvalidOperationException ex)
                {
                    Logger.LogError(message: "Getting new update failed with error.", exception: ex);
                    Cleanup();
                    break;
                }
                catch (Exception ex)
                {
                    Logger.LogError(message: "Getting new update failed with error.", exception: ex);
                }
            } while (tries < MaxTries);
        }

        UpdateAvailableToGet?.Invoke(sender: this, e: updateStatus);
    }

    /// <summary>
    ///     Get or download installer for the newest release.
    /// </summary>
    /// <param name="targetVersion">This does not contains the revision number, because backend always sends zero.</param>
    private async Task<(string filePath, Version newVersion)> GetInstallerAsync(
        Version targetVersion
    )
    {
        var result = await GetLatestReleaseFromGithubAsync(targetVersion: targetVersion)
            .ConfigureAwait(continueOnCapturedContext: false);
        /*        var sha256SumsFilePath = Path.Combine(InstallerDir, "SHA256SUMS.asc");*/

        // This will throw InvalidOperationException in case of invalid signature.
        /*  await DownloadAndValidateWasabiSignatureAsync(sha256SumsFilePath, result.Sha256SumsUrl, result.WasabiSigUrl).ConfigureAwait(false);*/

        var installerFilePath = Path.Combine(path1: InstallerDir, path2: result.InstallerFileName);

        try
        {
            if (!File.Exists(path: installerFilePath))
            {
                EnsureToRemoveCorruptedFiles();

                // This should also be done using Tor.
                // TODO: https://github.com/zkSNACKs/WalletWasabi/issues/8800
                Logger.LogInfo(message: $"Trying to download new version: {result.LatestVersion}");
                using HttpClient httpClient = new();

                // Get file stream and copy it to downloads folder to access.
                using var stream = await httpClient
                    .GetStreamAsync(requestUri: result.InstallerDownloadUrl, cancellationToken: CancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
                Logger.LogInfo(message: "Installer downloaded, copying...");

                await CopyStreamContentToFileAsync(stream: stream, filePath: installerFilePath)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            /* var expectedHash = await GetHashFromSha256SumsFileAsync(result.InstallerFileName, sha256SumsFilePath).ConfigureAwait(false);
             await VerifyInstallerHashAsync(installerFilePath, expectedHash).ConfigureAwait(false);*/
        }
        catch (IOException)
        {
            CancellationToken.ThrowIfCancellationRequested();
            throw;
        }

        return (installerFilePath, result.LatestVersion);
    }

    private async Task VerifyInstallerHashAsync(
        string installerFilePath
        , string expectedHash
    )
    {
    }

    /*    private async Task<string> GetHashFromSha256SumsFileAsync(string installerFileName, string sha256SumsFilePath)
        {
            var lines = await File.ReadAllLinesAsync(sha256SumsFilePath).ConfigureAwait(false);
            var correctLine = lines.FirstOrDefault(line => line.Contains(installerFileName));
            if (correctLine == null)
            {
                throw new InvalidOperationException($"{installerFileName} was not found.");
            }
            return correctLine.Split(" ")[0];
        }*/

    private async Task CopyStreamContentToFileAsync(
        Stream stream
        , string filePath
    )
    {
        if (File.Exists(path: filePath))
        {
            return;
        }

        var tmpFilePath = $"{filePath}.tmp";
        IoHelpers.EnsureContainingDirectoryExists(fileNameOrPath: tmpFilePath);
        using (var file = File.OpenWrite(path: tmpFilePath))
        {
            await stream.CopyToAsync(destination: file, cancellationToken: CancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            // Closing the file to rename.
            file.Close();
        }

        ;
        File.Move(sourceFileName: tmpFilePath, destFileName: filePath);
    }

    private async Task<(Version LatestVersion, string InstallerDownloadUrl, string
            InstallerFileName /*, string Sha256SumsUrl, string WasabiSigUrl*/)>
        GetLatestReleaseFromGithubAsync(
            Version targetVersion
        )
    {
        using var message = new HttpRequestMessage(method: HttpMethod.Get, requestUri: AppConstants.ReleaseURL);
        message.Headers.UserAgent.Add(item: new ProductInfoHeaderValue(productName: "ProjetoAcessibilidade"
            , productVersion: "2.0"));
        var response = await HttpClient.SendAsync(request: message, cancellationToken: CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        var jsonResponse = JObject.Parse(json: await response.Content
            .ReadAsStringAsync(cancellationToken: CancellationToken).ConfigureAwait(continueOnCapturedContext: false));

        var softwareVersion = jsonResponse[propertyName: "tag_name"]?.ToString() ??
                              throw new InvalidDataException(
                                  message: "Endpoint gave back wrong json data or it's changed.");

        // "tag_name" will have a 'v' at the beggining, needs to be removed.
        Version githubVersion = new(version: softwareVersion[1..]);
        Version shortGithubVersion =
            new(major: githubVersion.Major, minor: githubVersion.Minor, build: githubVersion.Build);
        if (targetVersion != shortGithubVersion)
        {
            throw new InvalidDataException(
                message:
                "Target version from backend does not match with the latest GitHub release. This should be impossible.");
        }

        // Get all asset names and download urls to find the correct one.
        var assetsInfos = jsonResponse[propertyName: "assets"]?.Children().ToList() ??
                          throw new InvalidDataException(message: "Missing assets from response.");
        List<string> assetDownloadUrls = new();
        foreach (var asset in assetsInfos)
        {
            assetDownloadUrls.Add(item: asset[key: "browser_download_url"]?.ToString() ??
                                        throw new InvalidDataException(message: "Missing download url from response."));
        }

        /*  var sha256SumsUrl = assetDownloadUrls.Where(url => url.Contains("SHA256SUMS.asc")).First();
          var wasabiSigUrl = assetDownloadUrls.Where(url => url.Contains("SHA256SUMS.wasabisig")).First();*/

        var (url, fileName) = GetAssetToDownload(urls: assetDownloadUrls);

        return (githubVersion, url, fileName /*, sha256SumsUrl, wasabiSigUrl*/);
    }

    private async Task DownloadAndValidateWasabiSignatureAsync(
        string sha256SumsFilePath
        , string sha256SumsUrl
        , string wasabiSigUrl
    )
    {
        var wasabiSigFilePath = Path.Combine(path1: InstallerDir, path2: "SHA256SUMS.wasabisig");

        using HttpClient httpClient = new();

        try
        {
            using (var stream = await httpClient
                       .GetStreamAsync(requestUri: sha256SumsUrl, cancellationToken: CancellationToken)
                       .ConfigureAwait(continueOnCapturedContext: false))
            {
                await CopyStreamContentToFileAsync(stream: stream, filePath: sha256SumsFilePath)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }

            using (var stream = await httpClient
                       .GetStreamAsync(requestUri: wasabiSigUrl, cancellationToken: CancellationToken)
                       .ConfigureAwait(continueOnCapturedContext: false))
            {
                await CopyStreamContentToFileAsync(stream: stream, filePath: wasabiSigFilePath)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }
        catch (HttpRequestException exc)
        {
            var message = "";
            if (exc.StatusCode is HttpStatusCode.NotFound)
            {
                message = "Wasabi signature files were not found under the API.";
            }
            else
            {
                message = "Something went wrong while getting Wasabi signature files.";
            }

            throw new InvalidOperationException(message: message, innerException: exc);
        }
        catch (IOException)
        {
            // There's a chance to get IOException when closing Wasabi during stream copying. Throw OperationCancelledException instead.
            CancellationToken.ThrowIfCancellationRequested();
            throw;
        }
    }

    private (string url, string fileName) GetAssetToDownload(
        List<string> urls
    )
    {
        if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows))
        {
            var url = urls.Where(predicate: url => url.Contains(value: ".msi")).First();
            return (url, url.Split(separator: "/").Last());
        }

        if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.OSX))
        {
            var cpu = RuntimeInformation.ProcessArchitecture;
            if (cpu.ToString() == "Arm64")
            {
                var arm64url = urls.Where(predicate: url => url.Contains(value: "arm64.dmg")).First();
                return (arm64url, arm64url.Split(separator: "/").Last());
            }

            var url = urls.Where(predicate: url => url.Contains(value: ".dmg") && !url.Contains(value: "arm64"))
                .First();
            return (url, url.Split(separator: "/").Last());
        }

        if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Linux))
        {
            throw new InvalidOperationException(message: "For Linux, get the correct update manually.");
        }

        throw new InvalidOperationException(message: "OS not recognized, download manually.");
    }

    private void EnsureToRemoveCorruptedFiles()
    {
        DirectoryInfo folder = new(path: InstallerDir);
        if (folder.Exists)
        {
            var corruptedFiles = folder.GetFileSystemInfos()
                .Where(predicate: file => file.Extension.Equals(value: ".tmp"));
            foreach (var file in corruptedFiles)
            {
                File.Delete(path: file.FullName);
            }
        }
    }

    private void Cleanup()
    {
        try
        {
            var folder = new DirectoryInfo(path: InstallerDir);
            if (folder.Exists)
            {
                Directory.Delete(path: InstallerDir, recursive: true);
            }
        }
        catch (Exception exc)
        {
            Logger.LogError(message: "Failed to delete installer directory.", exception: exc);
        }
    }

    public event EventHandler<UpdateStatus>? UpdateAvailableToGet;

    public void StartInstallingNewVersion()
    {
        try
        {
            ProcessStartInfo startInfo;
            if (!File.Exists(path: InstallerPath))
            {
                throw new FileNotFoundException(message: InstallerPath);
            }

            if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows))
            {
                startInfo = ProcessStartInfoFactory.Make(processPath: InstallerPath, arguments: "", openConsole: true);
            }
            else
            {
                startInfo = new ProcessStartInfo
                {
                    FileName = InstallerPath, UseShellExecute = true, WindowStyle = ProcessWindowStyle.Normal
                };
            }

            using var p = Process.Start(startInfo: startInfo);

            if (p is null)
            {
                throw new InvalidOperationException(message: $"Can't start {nameof(p)} {startInfo.FileName}.");
            }

            if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.OSX))
            {
                // For MacOS, you need to start the process twice, first start => permission denied
                // TODO: find out why and fix.

                p!.WaitForExit(milliseconds: 5000);
                p.Start();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(message: "Failed to install latest release. File might be corrupted.", exception: ex);
        }
    }

    public void Initialize(
        UpdateChecker updateChecker
        , CancellationToken cancelationToken
    )
    {
        UpdateChecker = updateChecker;
        CancellationToken = cancelationToken;
        updateChecker.UpdateStatusChanged += UpdateChecker_UpdateStatusChangedAsync;
    }
}