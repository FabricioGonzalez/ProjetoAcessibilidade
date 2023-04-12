using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace ProjectAvalonia.Features.Settings.ViewModels;

[NavigationMetaData(
    Title = "Opções Avançadas",
    Caption = "Gerenciamento de Opções Avançadas",
    Order = 2,
    Category = "Opções",
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

        this.WhenAnyValue(property1: x => x.EnableGpu)
            .ObserveOn(scheduler: RxApp.TaskpoolScheduler)
            .Throttle(dueTime: TimeSpan.FromMilliseconds(value: ThrottleTime))
            .Skip(count: 1)
            .Subscribe(onNext: _ => Save());
    }

    protected override void EditConfigOnSave(
        Config config
    ) => config.EnableGpu = EnableGpu;
}