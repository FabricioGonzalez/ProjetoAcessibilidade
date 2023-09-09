using System.Threading.Tasks;
using Common;

namespace ProjectAvalonia.Common.Helpers;

public static class MacOsStartupHelper
{
    private static readonly string AddCmd =
        $"osascript -e \' tell application \"System Events\" to make new login item at end with properties {{name:\"{Constants.AppName}\", path:\"/Applications/{Constants.AppName}.app\", hidden:true}} \'";

    private static readonly string DeleteCmd =
        $"osascript -e \' tell application \"System Events\" to delete login item \"{Constants.AppName}\" \'";

    public static async Task AddOrRemoveLoginItemAsync(
        bool runOnSystemStartup
    )
    {
        if (runOnSystemStartup)
        {
            await EnvironmentHelpers.ShellExecAsync(cmd: AddCmd).ConfigureAwait(continueOnCapturedContext: false);
        }
        else
        {
            await EnvironmentHelpers.ShellExecAsync(cmd: DeleteCmd).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}