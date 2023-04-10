using System;
using System.IO;
using System.Threading.Tasks;
using Common;

namespace ProjectAvalonia.Common.Helpers;

public static class LinuxStartupHelper
{
    public static async Task AddOrRemoveDesktopFileAsync(
        bool runOnSystemStartup
    )
    {
        var pathToDir = Path.Combine(path1: Environment.GetFolderPath(folder: Environment.SpecialFolder.UserProfile)
            , path2: ".config", path3: "autostart");
        var pathToDesktopFile = Path.Combine(path1: pathToDir, path2: $"{Constants.AppName}.desktop");

        IoHelpers.EnsureContainingDirectoryExists(fileNameOrPath: pathToDesktopFile);

        if (runOnSystemStartup)
        {
            var pathToExec = EnvironmentHelpers.GetExecutablePath();

            var pathToExecWithArgs = $"{pathToExec} {StartupHelper.SilentArgument}";

            IoHelpers.EnsureFileExists(filePath: pathToExec);

            var fileContents = string.Join(
                separator: "\n",
                "[Desktop Entry]",
                $"Name={Constants.AppName}",
                "Type=Application",
                $"Exec={pathToExecWithArgs}",
                "Hidden=false",
                "Terminal=false",
                "X-GNOME-Autostart-enabled=true");

            await File.WriteAllTextAsync(path: pathToDesktopFile, contents: fileContents)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        else
        {
            File.Delete(path: pathToDesktopFile);
        }
    }
}