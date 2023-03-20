using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Http;
using ProjectAvalonia.Common.Http.Extensions;
using ProjectAvalonia.Common.Models;

using Logger = ProjectAvalonia.Logging.Logger;

namespace ProjectAvalonia.Common.Services;

public class AppClient
{
    public AppClient(IHttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    private IHttpClient HttpClient
    {
        get;
    }

    public async Task<Version> GetVersionFromGithub()
    {
        using HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, AppConstants.ReleaseURL);
        message.Headers.UserAgent.Add(new ProductInfoHeaderValue("ProjetoAcessibilidade", "2.0"));
        var response = await HttpClient.SendAsync(message, CancellationToken.None)
            .ConfigureAwait(false);
        JObject jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync(CancellationToken.None)
            .ConfigureAwait(false));

        var softwareVersion = jsonResponse["tag_name"]?.ToString()
            ?? throw new InvalidDataException("Endpoint gave back wrong json data or it's changed.");

        // "tag_name" will have a 'v' at the beggining, needs to be removed.
        Version githubVersion = new(softwareVersion[1..]);
        Version shortGithubVersion = new(githubVersion.Major, githubVersion.Minor, githubVersion.Build);

        return shortGithubVersion;
    }

    public async Task<(Version ClientVersion, ushort BackendMajorVersion, Version LegalDocumentsVersion)> GetVersionsAsync(CancellationToken cancel)
    {
        try
        {
            using HttpResponseMessage response =
            await HttpClient
            .SendAsync(method: HttpMethod.Get, relativeUri: "", cancellationToken: cancel)
            .ConfigureAwait(continueOnCapturedContext: false);

            using HttpContent content = response.EnsureSuccessStatusCode().Content;
            var resp = await content.ReadAsJsonAsync<string>().ConfigureAwait(continueOnCapturedContext: false);

            return (Version.Parse(input: "1"), ushort.Parse(s: "1"), Version.Parse(input: "1"));
        }

        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return (Version.Parse(input: "1.0.0"), ushort.Parse(s: "1"), Version.Parse(input: "1.0.0"));
        }
    }
    public async Task<UpdateStatus> CheckUpdatesAsync(CancellationToken cancel)
    {
        var (clientVersion, backendMajorVersion, legalDocumentsVersion) =
            await GetVersionsAsync(cancel).ConfigureAwait(false);

        var appVersion = await GetVersionFromGithub().ConfigureAwait(false);

        var currentVersion = Version.Parse(ServicesConfig.Config.AppVersion);

        var clientUpToDate = /*AppConstants.ClientVersion >= clientVersion ||*/ currentVersion >= appVersion; // If the client version locally is greater than or equal to the backend's reported client version, then good.
        var backendCompatible = int.Parse(AppConstants.ClientSupportBackendVersionMax)
        >= backendMajorVersion && backendMajorVersion
        >= int.Parse(AppConstants.ClientSupportBackendVersionMin);
        // If ClientSupportBackendVersionMin <= backend major <= ClientSupportBackendVersionMax, then our software is compatible.
        var currentBackendMajorVersion = backendMajorVersion;

        if (backendCompatible)
        {
            // Only refresh if compatible.
            ApiVersion = currentBackendMajorVersion;
        }

        return new UpdateStatus(
            backendCompatible: true,
            clientUpToDate: clientUpToDate,
            legalDocumentsVersion: legalDocumentsVersion,
            currentBackendMajorVersion: currentBackendMajorVersion,
            clientVersion: appVersion);
    }

    public static ushort ApiVersion { get; private set; } = ushort.Parse(AppConstants.BackendMajorVersion);
}
