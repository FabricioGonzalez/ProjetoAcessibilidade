using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ProjectAvalonia.Common.Helpers;

public static class StartupHelper
{
    public static readonly string SilentArgument = "startsilent";

    public static async Task ModifyStartupSettingAsync(
        bool runOnSystemStartup
    )
    {
        if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows))
        {
            WindowsStartupHelper.AddOrRemoveRegistryKey(runOnSystemStartup: runOnSystemStartup);
        }
        else if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Linux))
        {
            await LinuxStartupHelper.AddOrRemoveDesktopFileAsync(runOnSystemStartup: runOnSystemStartup)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        else if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.OSX))
        {
            await MacOsStartupHelper.AddOrRemoveLoginItemAsync(runOnSystemStartup: runOnSystemStartup)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}