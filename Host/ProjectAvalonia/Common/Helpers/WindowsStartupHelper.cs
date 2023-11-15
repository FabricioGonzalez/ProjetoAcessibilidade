using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ProjectAvalonia.Common.Helpers;

public static class WindowsStartupHelper
{
    private const string KeyPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

    public static void AddOrRemoveRegistryKey(
        bool runOnSystemStartup
    )
    {
        if (!RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows))
        {
            throw new InvalidOperationException(message: "Registry modification can only be done on Windows.");
        }

        var pathToExeFile = EnvironmentHelpers.GetExecutablePath();

        var pathToExecWithArgs = $"{pathToExeFile} {StartupHelper.SilentArgument}";

        if (!File.Exists(path: pathToExeFile))
        {
            throw new InvalidOperationException(message: $"Path: {pathToExeFile} does not exist.");
        }

        using var key = Registry.CurrentUser.OpenSubKey(name: KeyPath, writable: true) ??
                        throw new InvalidOperationException(message: "Registry operation failed.");
        if (runOnSystemStartup)
        {
            key.SetValue(name: nameof(ProjectAvalonia), value: pathToExecWithArgs);
        }
        else
        {
            key.DeleteValue(name: nameof(ProjectAvalonia), throwOnMissingValue: false);
        }
    }
}