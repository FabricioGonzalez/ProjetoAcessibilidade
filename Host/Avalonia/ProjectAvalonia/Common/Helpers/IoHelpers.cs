using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ProjectAvalonia.Logging;

namespace ProjectAvalonia.Common.Helpers;

public static class IoHelpers
{
    /// <summary>
    ///     Attempts to delete <paramref name="directory" /> with retry feature to allow File Explorer or any other
    ///     application that holds the directory handle of the <paramref name="directory" /> to release it.
    /// </summary>
    /// <see
    ///     href="https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true/14933880#44324346" />
    public static async Task<bool> TryDeleteDirectoryAsync(
        string directory
        , int maxRetries = 10
        , int millisecondsDelay = 100
    )
    {
        Guard.NotNull(parameterName: nameof(directory), value: directory);

        if (maxRetries < 1)
        {
            throw new ArgumentOutOfRangeException(paramName: nameof(maxRetries));
        }

        if (millisecondsDelay < 1)
        {
            throw new ArgumentOutOfRangeException(paramName: nameof(millisecondsDelay));
        }

        for (var i = 0; i < maxRetries; ++i)
        {
            try
            {
                if (Directory.Exists(path: directory))
                {
                    Directory.Delete(path: directory, recursive: true);
                }

                return true;
            }
            catch (DirectoryNotFoundException e)
            {
                Logger.LogDebug(message: $"Directory was not found: {e}");

                // Directory does not exist. So the operation is trivially done.
                return true;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                await Task.Delay(millisecondsDelay: millisecondsDelay).ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        return false;
    }

    public static void EnsureContainingDirectoryExists(
        string fileNameOrPath
    )
    {
        var fullPath =
            Path.GetFullPath(path: fileNameOrPath); // No matter if relative or absolute path is given to this.
        var dir = Path.GetDirectoryName(path: fullPath);
        EnsureDirectoryExists(dir: dir);
    }

    /// <summary>
    ///     It's like Directory.CreateDirectory, but does not fail when root is given.
    /// </summary>
    public static void EnsureDirectoryExists(
        string? dir
    )
    {
        if (!string.IsNullOrWhiteSpace(value: dir)) // If root is given, then do not worry.
        {
            Directory.CreateDirectory(path: dir); // It does not fail if it exists.
        }
    }

    public static void EnsureFileExists(
        string filePath
    )
    {
        if (!File.Exists(path: filePath))
        {
            EnsureContainingDirectoryExists(fileNameOrPath: filePath);

            File.Create(path: filePath)?.Dispose();
        }
    }

    public static bool CheckIfFileExists(
        string filePath
    ) => File.Exists(path: filePath);

    public static void OpenFolderInFileExplorer(
        string dirPath
    )
    {
        if (Directory.Exists(path: dirPath))
        {
            // RuntimeInformation.OSDescription on WSL2 reports a string like:
            // 'Linux 5.10.102.1-microsoft-standard-WSL2 #1 SMP Wed Mar 2 00:30:59 UTC 2022'
            if (!RuntimeInformation.OSDescription.ToString(provider: CultureInfo.InvariantCulture)
                    .Contains(value: "WSL2"))
            {
                using var process = Process.Start(startInfo: new ProcessStartInfo
                {
                    FileName = RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows)
                        ? "explorer.exe"
                        : RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.OSX)
                            ? "open"
                            : "xdg-open"
                    , Arguments = RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows)
                        ? $"\"{dirPath}\""
                        : dirPath
                    , CreateNoWindow = true
                });
            }
        }
    }

    public static async Task OpenBrowserAsync(
        string url
    )
    {
        if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Linux))
        {
            // If no associated application/json MimeType is found xdg-open opens retrun error
            // but it tries to open it anyway using the console editor (nano, vim, other..)
            await EnvironmentHelpers.ShellExecAsync(cmd: $"xdg-open {url}", waitForExit: false)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        else
        {
            using var process = Process.Start(startInfo: new ProcessStartInfo
            {
                FileName = RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows) ? url : "open"
                , Arguments = RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.OSX) ? $"-e {url}" : ""
                , CreateNoWindow = true
                , UseShellExecute = RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows)
            });
        }
    }

    public static void CopyFilesRecursively(
        DirectoryInfo source
        , DirectoryInfo target
    )
    {
        foreach (var dir in source.GetDirectories())
        {
            CopyFilesRecursively(source: dir, target: target.CreateSubdirectory(path: dir.Name));
        }

        foreach (var file in source.GetFiles())
        {
            file.CopyTo(destFileName: Path.Combine(path1: target.FullName, path2: file.Name));
        }
    }

    public static void CreateOrOverwriteFile(
        string path
    )
    {
        using var _ = File.Create(path: path);
    }
}