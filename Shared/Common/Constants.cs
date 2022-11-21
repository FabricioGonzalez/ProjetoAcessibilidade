namespace Common;
public static class Constants
{
#if DEBUG
    public static string AppName = "Gestor de Projeto ARPA";
    public static string AppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

    public static string AppCacheFolder = Path.Combine(AppFolder, "CacheData");
    public static string AppHistoryFolder = Path.Combine(AppCacheFolder, "RecentlyOpened");
    public static string AppUnclosedItemsFolder = Path.Combine(AppCacheFolder, "UnclosedItems");

    public static string AppSettingsFolder = Path.Combine(AppFolder, "Settings");
    public static string AppUISettings = Path.Combine(AppSettingsFolder, "UISettings");

    public static string AppTemplatesFolder = Path.Combine(AppFolder, "Templates");
    public static string AppItemsTemplateFolder = Path.Combine(AppTemplatesFolder, "Itens");

    public const string AppUserProjectItemsFolder = "Itens";
    public const string AppProjectSolutionExtension = ".pjra";
    public const string AppProjectTemplateExtension = ".xml";
    public const string AppProjectItemExtension = ".pjrd";
#else
    public static string AppName = "Gestor de Projeto ARPA";
    public static string AppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

    public static string AppCacheFolder = Path.Combine(AppFolder, "CacheData");
    public static string AppHistoryFolder = Path.Combine(AppCacheFolder, "RecentlyOpened");
    public static string AppUnclosedItemsFolder = Path.Combine(AppCacheFolder, "UnclosedItems");

    public static string AppSettingsFolder = Path.Combine(AppFolder, "Settings");
    public static string AppUISettings = Path.Combine(AppSettingsFolder, "UISettings");

    public static string AppTemplatesFolder = Path.Combine(AppFolder, "Templates");
    public static string AppItemsTemplateFolder = Path.Combine(AppTemplatesFolder, "Itens");

    public const string AppUserProjectItemsFolder = "Itens";
#endif
}
