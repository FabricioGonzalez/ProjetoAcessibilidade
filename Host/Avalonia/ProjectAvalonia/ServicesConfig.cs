using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Interfaces;
using ProjectAvalonia.Common.Services;

namespace ProjectAvalonia;

public static class ServicesConfig
{
    public static string DataDir
    {
        get;
        private set;
    }

    public static ILanguageManager LanguageManager
    {
        get;
        private set;
    }

    public static Config Config
    {
        get;
        private set;
    }

    public static HostedServices HostedServices
    {
        get;
        private set;
    }

    public static UiConfig UiConfig
    {
        get;
        private set;
    }

    public static UpdateManager UpdateManager
    {
        get;
        private set;
    }

    public static bool IsInitialized
    {
        get;
        private set;
    }

    /// <summary>
    ///     Initializes global services used by fluent project.
    /// </summary>
    /// <param name="global">The global instance.</param>
    /// <param name="singleInstanceChecker">The singleInstanceChecker instance.</param>
    public static void Initialize(
        Global global
    )
    {
        Guard.NotNull(parameterName: nameof(global.DataDir), value: global.DataDir);
        Guard.NotNull(parameterName: nameof(global.Config), value: global.Config);
        Guard.NotNull(parameterName: nameof(global.HostedServices), value: global.HostedServices);
        Guard.NotNull(parameterName: nameof(global.UiConfig), value: global.UiConfig);
        Guard.NotNull(parameterName: nameof(global.LanguageManager), value: global.LanguageManager);
        /*Guard.NotNull(nameof(global.UpdateManager), global.UpdateManager);*/

        DataDir = global.DataDir;
        Config = global.Config;
        HostedServices = global.HostedServices;
        UiConfig = global.UiConfig;
        UpdateManager = global.UpdateManager;
        LanguageManager = global.LanguageManager;
        LanguageManager.SetLanguage(languageCode: global.Config.AppLanguage);
        IsInitialized = true;
    }
}