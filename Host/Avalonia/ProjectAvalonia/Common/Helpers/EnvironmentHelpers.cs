using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.Win32;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Microservices;
using ProjectAvalonia.Logging;

namespace ProjectAvalonia.Common.Helpers;

public static class EnvironmentHelpers
{
    // appName, dataDir
    private static ConcurrentDictionary<string, string> DataDirDict
    {
        get;
    } = new();

    // Do not change the output of this function. Backwards compatibility depends on it.
    public static string GetDataDir(
        string appName
    )
    {
        if (DataDirDict.TryGetValue(key: appName, value: out var dataDir))
        {
            return dataDir;
        }

        string directory;

        if (!RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows))
        {
            var home = Environment.GetEnvironmentVariable(variable: "HOME");
            if (!string.IsNullOrEmpty(value: home))
            {
                directory = Path.Combine(path1: home, path2: "." + appName.ToLowerInvariant());
                Logger.LogInfo(
                    message: $"Using HOME environment variable for initializing application data at `{directory}`.");
            }
            else
            {
                throw new DirectoryNotFoundException(message: "Could not find suitable datadir.");
            }
        }
        else
        {
            var localAppData = Environment.GetEnvironmentVariable(variable: "APPDATA");
            if (!string.IsNullOrEmpty(value: localAppData))
            {
                directory = Path.Combine(path1: localAppData, path2: appName);
                Logger.LogInfo(
                    message: $"Using APPDATA environment variable for initializing application data at `{directory}`.");
            }
            else
            {
                throw new DirectoryNotFoundException(message: "Could not find suitable datadir.");
            }
        }

        if (Directory.Exists(path: directory))
        {
            DataDirDict.TryAdd(key: appName, value: directory);
            return directory;
        }

        Logger.LogInfo(message: $"Creating data directory at `{directory}`.");
        Directory.CreateDirectory(path: directory);

        DataDirDict.TryAdd(key: appName, value: directory);
        return directory;
    }


    // This method removes the path and file extension.
    //
    // Given Wasabi releases are currently built using Windows, the generated assemblies contain
    // the hard coded "C:\Users\User\Desktop\WalletWasabi\.......\FileName.cs" string because that
    // is the real path of the file, it doesn't matter what OS was targeted.
    // In Windows and Linux that string is a valid path and that means Path.GetFileNameWithoutExtension
    // can extract the file name but in the case of OSX the same string is not a valid path so, it assumes
    // the whole string is the file name.
    public static string ExtractFileName(
        string callerFilePath
    )
    {
        var lastSeparatorIndex = callerFilePath.LastIndexOf(value: "\\");
        if (lastSeparatorIndex == -1)
        {
            lastSeparatorIndex = callerFilePath.LastIndexOf(value: "/");
        }

        var fileName = callerFilePath;

        if (lastSeparatorIndex != -1)
        {
            lastSeparatorIndex++;
            fileName = callerFilePath[lastSeparatorIndex..]; // From lastSeparatorIndex until the end of the string.
        }

        var fileNameWithoutExtension =
            fileName.TrimEnd(trimString: ".cs", comparisonType: StringComparison.InvariantCultureIgnoreCase);
        return fileNameWithoutExtension;
    }

    /// <summary>
    ///     Executes a command with Bourne shell.
    ///     https://stackoverflow.com/a/47918132/2061103
    /// </summary>
    public static async Task ShellExecAsync(
        string cmd
        , bool waitForExit = true
    )
    {
        var escapedArgs = cmd.Replace(oldValue: "\"", newValue: "\\\"");

        var startInfo = new ProcessStartInfo
        {
            FileName = "/usr/bin/env", Arguments = $"sh -c \"{escapedArgs}\"", RedirectStandardOutput = true
            , UseShellExecute = false, CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden
        };

        if (waitForExit)
        {
            using var process = new ProcessAsync(startInfo: startInfo);
            process.Start();

            await process.WaitForExitAsync(cancellationToken: CancellationToken.None)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (process.ExitCode != 0)
            {
                Logger.LogError(
                    message:
                    $"{nameof(ShellExecAsync)} command: {cmd} exited with exit code: {process.ExitCode}, instead of 0.");
            }
        }
        else
        {
            using var process = Process.Start(startInfo: startInfo);
        }
    }

    public static bool IsFileTypeAssociated(
        string fileExtension
    )
    {
        // Source article: https://edi.wang/post/2019/3/4/read-and-write-windows-registry-in-net-core

        if (!RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows))
        {
            throw new InvalidOperationException(message: "Operation only supported on windows.");
        }

        fileExtension = fileExtension.TrimStart(trimChar: '.'); // Remove . if added by the caller.

        using (var key = Registry.ClassesRoot.OpenSubKey(name: $".{fileExtension}"))
        {
            // Read the (Default) value.
            if (key?.GetValue(name: null) is not null)
            {
                return true;
            }
        }

        return false;
    }

    public static string GetFullBaseDirectory()
    {
        var fullBaseDirectory = Path.GetFullPath(path: AppContext.BaseDirectory);

        if (!RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows))
        {
            if (!fullBaseDirectory.StartsWith(value: '/'))
            {
                fullBaseDirectory = fullBaseDirectory.Insert(startIndex: 0, value: "/");
            }
        }

        return fullBaseDirectory;
    }

    public static string GetExecutablePath()
    {
        var fullBaseDir = GetFullBaseDirectory();
        var executablePath = Path.Combine(path1: fullBaseDir, path2: Constants.ExecutableName);
        executablePath = RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows)
            ? $"{executablePath}.exe"
            : $"{executablePath}";
        if (File.Exists(path: executablePath))
        {
            return executablePath;
        }

        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name ??
                           throw new NullReferenceException(message: "Assembly or Assembly's Name was null.");
        var fluentExecutable = Path.Combine(path1: fullBaseDir, path2: assemblyName);
        return RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows)
            ? $"{fluentExecutable}.exe"
            : $"{fluentExecutable}";
    }

    public static string GetExecutableVersion()
    {
        var versInfo = FileVersionInfo.GetVersionInfo(fileName: GetExecutablePath());

        return versInfo?.ProductVersion;
    }
}