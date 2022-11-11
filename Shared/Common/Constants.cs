namespace Common;
public static class Constants
{

    public static string AppFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    public static string AppCacheFolder = Path.Combine(AppFolder, "CacheData");
    public static string AppHistoryFolder = Path.Combine(AppCacheFolder, "RecentlyOpened");
    public static string AppUnclosedItemsFolder = Path.Combine(AppCacheFolder, "UnclosedItems");

    public static string AppSettingsFolder = Path.Combine(AppFolder, "Settings");
    public static string AppUISettings = Path.Combine(AppSettingsFolder, "UISettings");

    public static string AppTemplatesFolder = Path.Combine(AppFolder, "Templates");
    public static string AppItemsTemplateFolder = Path.Combine(AppTemplatesFolder, "Itens");

    public const string AppUserProjectItemsFolder = "Itens";
}
