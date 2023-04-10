using System;
using System.Reactive.Concurrency;
using ProjectAvalonia.Common.Models;
using ProjectAvalonia.Logging;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.Features.Settings.ViewModels;

public abstract class SettingsTabViewModelBase : RoutableViewModel
{
    public const int ThrottleTime = 500;

    protected SettingsTabViewModelBase()
    {
        ConfigOnOpen = new Config(filePath: ServicesConfig.Config.FilePath);
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

        var config = new Config(filePath: ConfigOnOpen.FilePath);

        RxApp.MainThreadScheduler.Schedule(
            action: () =>
            {
                try
                {
                    lock (ConfigLock)
                    {
                        config.LoadFile();
                        EditConfigOnSave(config: config);
                        config.ToFile();

                        OnConfigSaved();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogDebug(exception: ex);
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
            sender: typeof(SettingsTabViewModelBase),
            e: new RestartNeededEventArgs
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

        var currentConfig = new Config(filePath: ConfigOnOpen.FilePath);
        currentConfig.LoadFile();

        var isRestartNeeded = !ConfigOnOpen.AreDeepEqual(otherConfig: currentConfig);

        return isRestartNeeded;
    }
}