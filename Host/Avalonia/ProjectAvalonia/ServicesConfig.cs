using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Services;

namespace ProjectAvalonia;

public static class ServicesConfig
{
    public static string DataDir
    {
        get; private set;
    }

    public static Config Config
    {
        get; private set;
    }

    public static HostedServices HostedServices
    {
        get; private set;
    }

    public static UiConfig UiConfig
    {
        get; private set;
    }

    public static UpdateManager UpdateManager
    {
        get; private set;
    }

    public static bool IsInitialized
    {
        get; private set;
    }

    /// <summary>
    /// Initializes global services used by fluent project.
    /// </summary>
    /// <param name="global">The global instance.</param>
    /// <param name="singleInstanceChecker">The singleInstanceChecker instance.</param>
    public static void Initialize(Global global)
    {
        Guard.NotNull(nameof(global.DataDir), global.DataDir);
        Guard.NotNull(nameof(global.Config), global.Config);
        Guard.NotNull(nameof(global.HostedServices), global.HostedServices);
        Guard.NotNull(nameof(global.UiConfig), global.UiConfig);
        /*Guard.NotNull(nameof(global.UpdateManager), global.UpdateManager);*/

        DataDir = global.DataDir;
        Config = global.Config;
        HostedServices = global.HostedServices;
        UiConfig = global.UiConfig;
        /*UpdateManager = global.UpdateManager;*/

        IsInitialized = true;
    }
}
