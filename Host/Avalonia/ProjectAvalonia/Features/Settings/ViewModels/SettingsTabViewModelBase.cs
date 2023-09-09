using System;
using System.Reactive.Concurrency;
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Common.Models;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.Features.Settings.ViewModels;

public abstract class SettingsTabViewModelBase : RoutableViewModel
{
    public const int ThrottleTime = 500;

    protected SettingsTabViewModelBase()
    {
        ConfigOnOpen = new Config(ServicesConfig.Config.FilePath);
        ConfigOnOpen.LoadFile();
    }

    public static Config? ConfigOnOpen
    {
        get;
        set;
    }

    private static object ConfigLock
    {
        get;
    } = new();

    public static event EventHandler<RestartNeededEventArgs>? RestartNeeded;

    protected void Save()
    {
        if (Validations.Any || ConfigOnOpen is null)
        {
            return;
        }

        var config = new Config(ConfigOnOpen.FilePath);

        RxApp.MainThreadScheduler.Schedule(
            () =>
            {
                try
                {
                    lock (ConfigLock)
                    {
                        config.LoadFile();
                        EditConfigOnSave(config);
                        config.ToFile();

                        OnConfigSaved();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogDebug(ex);
                }
            });
    }

    protected abstract void EditConfigOnSave(
        Config config
    );

    private static void OnConfigSaved()
    {
        var isRestartNeeded = CheckIfRestartIsNeeded();

        RestartNeeded?.Invoke(
            typeof(SettingsTabViewModelBase),
            new RestartNeededEventArgs
            {
                IsRestartNeeded = isRestartNeeded
            });
    }

    public static bool CheckIfRestartIsNeeded()
    {
        if (ConfigOnOpen is null)
        {
            return false;
        }

        var currentConfig = new Config(ConfigOnOpen.FilePath);
        currentConfig.LoadFile();

        var isRestartNeeded = !ConfigOnOpen.AreDeepEqual(currentConfig);

        return isRestartNeeded;
    }
}