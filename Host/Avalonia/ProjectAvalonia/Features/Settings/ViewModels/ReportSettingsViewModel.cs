using System;
using System.Reactive.Linq;

using ProjectAvalonia.ViewModels;

using ReactiveUI;

namespace ProjectAvalonia.Features.Settings.ViewModels;

[NavigationMetaData(
    Title = "Configurações do Relatório",
    Caption = "Gerencia Configurações do Relatório",
    Order = 2,
    LocalizedTitle = "ReportSettingsViewNavLabel",
    Category = "Opções",
    Keywords = new[]
    {
        "Settings", "General", "Bitcoin", "Dark", "Mode", "Run", "Computer", "System", "Start", "Background", "Close"
        , "Auto", "Copy", "Paste", "Addresses", "Custom", "Change", "Address", "Fee", "Display", "Format", "BTC", "sats"
    },
    IconName = "settings_general_regular")]
public partial class ReportSettingsViewModel : SettingsTabViewModelBase
{
    public ReportSettingsViewModel()
    {
        SetTitle("ReportSettingsViewNavLabel");

        LawContent = ServicesConfig.UiConfig.DefaultLawContent;

        this.WhenAnyValue(x => x.LawContent)
           .ObserveOn(RxApp.TaskpoolScheduler)
           .Skip(1)
           .Subscribe(x => ServicesConfig.UiConfig.DefaultLawContent = x);

    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
    }

    public override MenuViewModel? ToolBar
    {
        get;
    }

    protected override void EditConfigOnSave(Config config)
    {

    }

    private string lawContent;
    public string LawContent
    {
        get => lawContent;
        set => this.RaiseAndSetIfChanged(ref lawContent, value);
    }
}
