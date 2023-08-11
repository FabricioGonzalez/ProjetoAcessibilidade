namespace Common;

public static class Constants
{
    public const string SolutionName = "ProjetoAcessibilidade";
    public const string AppName = "Gestor de Projeto ARPA";

    public const string ShuttingDownLabel = "O Sistema está desligando...";

    public const string ExecutableName = "Gestor de Projeto ARPA";
    public const string AppSettingsFile = "Config.json";

    public const string AppUISettingsFile = "UiConfig.json";

    public const string AppDatabaseFileName = "GestorProjetosArpa.db";

    public const string AppProjectItemsFolderName = "Itens";
    public const string AppProjectRulesItemsFolderName = "Rules";
    public const string AppValidationRulesTemplateFolderName = "Validation Templates";
    public const string AppProjectSolutionExtension = ".prja";
    public const string AppProjectValidationTemplateExtension = ".vldr";
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
    public const string reportItemNomeEmpresa = "company_name";
    public const string reportItemEmail = "email";
    public const string reportItemEndereco = "address";
    public const string reportItemResponsavel = "responsable";
    public const string reportItemTelefone = "phone";
    public const string reportItemData = "report_data";
    public const string reportItemLogoPath = "logo_path";
    public const string reportItemSolutionName = "solution_name";

    public static string AppFolder = OperatingSystem.IsWindows()
        ? Path.Combine(
            path1: Environment.GetFolderPath(folder: Environment.SpecialFolder.ApplicationData),
            path2: AppName)
        : Path.Combine(
            path1: Environment.GetFolderPath(folder: Environment.SpecialFolder.LocalApplicationData),
            path2: AppName);

    public static string AppCacheFolder = Path.Combine(
        path1: AppFolder,
        path2: "CacheData");

    public static string DatabaseFolder = Path.Combine(
        path1: AppFolder,
        path2: "Database");

    public static string AppHistoryFolder = Path.Combine(
        path1: AppCacheFolder,
        path2: "RecentlyOpened");

    public static string AppUnclosedItemsFolder = Path.Combine(
        path1: AppCacheFolder,
        path2: "UnclosedItems");

    public static string AppSettingsFolder = Path.Combine(
        path1: AppFolder,
        path2: "Settings");

    public static string AppUISettings = Path.Combine(
        path1: AppSettingsFolder,
        path2: "UISettings");

    public static string AppLogsSettings = Path.Combine(
        path1: AppSettingsFolder,
        path2: "Logs");

    public static string AppTemplatesFolder = Path.Combine(
        path1: AppFolder,
        path2: "Templates");

    public static string AppItemsTemplateFolder =
        Path.Combine(
            path1: AppTemplatesFolder,
            path2: AppProjectItemsFolderName);

    public static string AppValidationRulesTemplateFolder =
        Path.Combine(
            path1: AppTemplatesFolder,
            path2: AppValidationRulesTemplateFolderName);
}
#if NET
#endif