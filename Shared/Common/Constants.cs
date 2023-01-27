namespace Common;
public static class Constants
{
    public const string AppName = "Gestor de Projeto ARPA";

    public const string ExecutableName = "Gestor de Projeto ARPA";
    public static string AppFolder = OperatingSystem.IsWindows()
        ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName)
        : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);

    public static string AppCacheFolder = Path.Combine(AppFolder, "CacheData");
    public static string AppHistoryFolder = Path.Combine(AppCacheFolder, "RecentlyOpened");
    public static string AppUnclosedItemsFolder = Path.Combine(AppCacheFolder, "UnclosedItems");

    public static string AppSettingsFolder = Path.Combine(AppFolder, "Settings");
    public static string AppUISettings = Path.Combine(AppSettingsFolder, "UISettings");
    public static string AppLogsSettings = Path.Combine(AppSettingsFolder, "Logs");

    public static string AppTemplatesFolder = Path.Combine(AppFolder, "Templates");
    public static string AppItemsTemplateFolder = Path.Combine(AppTemplatesFolder, AppProjectItemsFolderName);

    public const string AppProjectItemsFolderName = "Itens";
    public const string AppProjectSolutionExtension = ".prja";
    public const string AppProjectTemplateExtension = ".xml";
    public const string AppProjectItemExtension = ".prjd";


    public const string solutionRoot = "solution";

    public const string items_groups = "items_groups";
    public const string items_groupsItemGroup = "item_group";
    public const string items_groupsItemGroupAttributeName = "Name";
    public const string items_groups_item_group_item = "Item";
    public const string items_groups_item_group_item_id = "Id";
    public const string items_groups_item_group_item_name = "Name";
    public const string items_groups_item_group_item_template_name = "TemplateName";


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
