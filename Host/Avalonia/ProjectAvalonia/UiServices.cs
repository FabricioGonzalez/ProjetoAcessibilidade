using Common;

using ProjectAvalonia.Common.Helpers;

namespace ProjectAvalonia;

public static class UiServices
{
    public static void Initialize()
    {
        IoHelpers.EnsureContainingDirectoryExists(Constants.AppSettingsFolder);
        IoHelpers.EnsureContainingDirectoryExists(Constants.AppTemplatesFolder);
        IoHelpers.EnsureContainingDirectoryExists(Constants.AppItemsTemplateFolder);
        IoHelpers.EnsureContainingDirectoryExists(Constants.AppUnclosedItemsFolder);
        IoHelpers.EnsureContainingDirectoryExists(Constants.AppHistoryFolder);
        IoHelpers.EnsureContainingDirectoryExists(Constants.AppCacheFolder);
        IoHelpers.EnsureContainingDirectoryExists(Constants.AppLogsSettings);
    }
}
