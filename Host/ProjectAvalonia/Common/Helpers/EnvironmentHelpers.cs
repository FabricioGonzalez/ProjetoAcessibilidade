﻿using System;
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
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Common.Microservices;

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

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var home = Environment.GetEnvironmentVariable("HOME");
            if (!string.IsNullOrEmpty(home))
            {
                directory = Path.Combine(path1: home, path2: "." + appName.ToLowerInvariant());
                Logger.LogInfo(
                    $"Using HOME environment variable for initializing application data at `{directory}`.");
            }
            else
            {
                throw new DirectoryNotFoundException("Could not find suitable datadir.");
            }
        }
        else
        {
            var localAppData = Environment.GetEnvironmentVariable("APPDATA");
            if (!string.IsNullOrEmpty(localAppData))
            {
                directory = Path.Combine(path1: localAppData, path2: appName);
                Logger.LogInfo(
                    $"Using APPDATA environment variable for initializing application data at `{directory}`.");
            }
            else
            {
                throw new DirectoryNotFoundException("Could not find suitable datadir.");
            }
        }

        if (Directory.Exists(directory))
        {
            DataDirDict.TryAdd(key: appName, value: directory);
            return directory;
        }

        Logger.LogInfo($"Creating data directory at `{directory}`.");
        Directory.CreateDirectory(directory);

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
        var lastSeparatorIndex = callerFilePath.LastIndexOf("\\");
        if (lastSeparatorIndex == -1)
        {
            lastSeparatorIndex = callerFilePath.LastIndexOf("/");
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
            using var process = new ProcessAsync(startInfo);
            process.Start();

            await process.WaitForExitAsync(CancellationToken.None)
                .ConfigureAwait(false);

            if (process.ExitCode != 0)
            {
                Logger.LogError(
                    $"{nameof(ShellExecAsync)} command: {cmd} exited with exit code: {process.ExitCode}, instead of 0.");
            }
        }
        else
        {
            using var process = Process.Start(startInfo);
        }
    }

    public static bool IsFileTypeAssociated(
        string fileExtension
    )
    {
        // Source article: https://edi.wang/post/2019/3/4/read-and-write-windows-registry-in-net-core

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new InvalidOperationException("Operation only supported on windows.");
        }

        fileExtension = fileExtension.TrimStart('.'); // Remove . if added by the caller.

        using (var key = Registry.ClassesRoot.OpenSubKey($".{fileExtension}"))
        {
            // Read the (Default) value.
            if (key?.GetValue(null) is not null)
            {
                return true;
            }
        }

        return false;
    }

    public static string GetFullBaseDirectory()
    {
        var fullBaseDirectory = Path.GetFullPath(AppContext.BaseDirectory);

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (!fullBaseDirectory.StartsWith('/'))
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
        executablePath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? $"{executablePath}.exe"
            : $"{executablePath}";
        if (File.Exists(executablePath))
        {
            return executablePath;
        }

        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name ??
                           throw new NullReferenceException("Assembly or Assembly's Name was null.");
        var fluentExecutable = Path.Combine(path1: fullBaseDir, path2: assemblyName);
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? $"{fluentExecutable}.exe"
            : $"{fluentExecutable}";
    }

    public static string GetExecutableVersion()
    {
        var versInfo = FileVersionInfo.GetVersionInfo(GetExecutablePath());

        return $"{versInfo?.ProductMajorPart}.{versInfo?.ProductMinorPart}.{versInfo?.ProductBuildPart}";
    }
}