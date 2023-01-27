using System;
using System.Reactive.Linq;

using ReactiveUI;

namespace ProjectAvalonia.Features.Settings.ViewModels;

[NavigationMetaData(
    Title = "Advanced",
    Caption = "Manage advanced settings",
    Order = 2,
    Category = "Settings",
    Keywords = new[]
    {
            "Settings", "Advanced", "Enable", "GPU"
    },
    IconName = "settings_general_regular")]
public partial class AdvancedSettingsTabViewModel : SettingsTabViewModelBase
{
    [AutoNotify] private bool _enableGpu;

    public AdvancedSettingsTabViewModel()
    {
        _enableGpu = ServicesConfig.Config.EnableGpu;

        this.WhenAnyValue(x => x.EnableGpu)
            .ObserveOn(RxApp.TaskpoolScheduler)
            .Throttle(TimeSpan.FromMilliseconds(ThrottleTime))
            .Skip(1)
            .Subscribe(_ => Save());
    }

    protected override void EditConfigOnSave(Config config)
    {
        config.EnableGpu = EnableGpu;
    }
}
