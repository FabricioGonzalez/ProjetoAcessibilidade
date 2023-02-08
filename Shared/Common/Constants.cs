namespace Common;
public static class Constants
{
    public const string AppName = "Gestor de Projeto ARPA";

    public const string ShuttingDownLabel = "O Sistema está desligando...";

    public const string ExecutableName = "Gestor de Projeto ARPA";
    public static string AppFolder = OperatingSystem.IsWindows()
        ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName)
        : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);

    public static string AppCacheFolder = Path.Combine(AppFolder, "CacheData");
    public static string AppHistoryFolder = Path.Combine(AppCacheFolder, "RecentlyOpened");
    public static string AppUnclosedItemsFolder = Path.Combine(AppCacheFolder, "UnclosedItems");

    public static string AppSettingsFolder = Path.Combine(AppFolder, "Settings");
    public const string AppSettingsFile = "Config.json";

    public const string AppUISettingsFile = "UiConfig.json";
    public static string AppUISettings = Path.Combine(AppSettingsFolder, "UISettings");

    public static string AppLogsSettings = Path.Combine(AppSettingsFolder, "Logs");

    public static string AppTemplatesFolder = Path.Combine(AppFolder, "Templates");
    public static string AppItemsTemplateFolder = Path.Combine(AppTemplatesFolder, AppProjectItemsFolderName);

    public const string AppProjectItemsFolderName = "Itens";
    public const string AppProjectSolutionExtension = ".prja";
    public const string AppProjectTemplateExtension = ".xml";
    public const string AppProjectItemExtension = ".prjd";

    public const string solutionRoot = "solution";

    public const string project_items = "project_items";
    public const string items_groups = "items_groups";
    public const string items_groupsItemGroupAttributeName = "name";
    public const string items_groups_item = "item";
    public const string items_groups_item_id = "id";
    public const string items_groups_item_name = "name";
    public const string items_groups_item_item_path = "item_path";
    public const string items_groups_item_template_name = "template_name";


    public const string report = "report";
    public const string reportItemNomeEmpresa = "NomeEmpresa";
    public const string reportItemEmail = "Email";
    public const string reportItemEndereco = "Endereco";
    public const string reportItemResponsavel = "Responsavel";
    public const string reportItemTelefone = "Telefone";
    public const string reportItemData = "Data";
    public const string reportItemLogoPath = "LogoPath";
    public const string reportItemSolutionName = "SolutionName";
}
