using Common;
using ProjectAvalonia.Common.Helpers;

namespace ProjectAvalonia;

public static class UiServices
{
    public static void Initialize()
    {
        IoHelpers.EnsureContainingDirectoryExists(fileNameOrPath: Constants.AppSettingsFolder);
        IoHelpers.EnsureContainingDirectoryExists(fileNameOrPath: Constants.AppTemplatesFolder);
        IoHelpers.EnsureContainingDirectoryExists(fileNameOrPath: Constants.AppItemsTemplateFolder);
        IoHelpers.EnsureContainingDirectoryExists(fileNameOrPath: Constants.AppUnclosedItemsFolder);
        IoHelpers.EnsureContainingDirectoryExists(fileNameOrPath: Constants.AppHistoryFolder);
        IoHelpers.EnsureContainingDirectoryExists(fileNameOrPath: Constants.AppCacheFolder);
        IoHelpers.EnsureContainingDirectoryExists(fileNameOrPath: Constants.AppLogsSettings);
    }
}