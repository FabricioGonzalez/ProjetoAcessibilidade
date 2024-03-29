using System.Reactive;
using System.Reactive.Disposables;
using System.Windows.Input;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Models;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;

namespace ProjectAvalonia.Features.Settings.ViewModels;

[NavigationMetaData(
    Title = "Settings",
    Caption = "Manage appearance, privacy and other settings",
    Order = 1,
    LocalizedTitle = "SettingsViewNavLabel",
    Category = "General",
    Keywords = new[] { "Settings", "General", "User", "Interface", "Advanced" },
    IconName = "nav_settings_24_regular",
    IconNameFocused = "nav_settings_24_filled",
    Searchable = false,
    NavBarPosition = NavBarPosition.Bottom,
    NavigationTarget = NavigationTarget.DialogScreen)]
public partial class SettingsPageViewModel : DialogViewModelBase<Unit>
{
    [AutoNotify] private bool _isModified;
    [AutoNotify] private int _selectedTab;

    public SettingsPageViewModel()
    {
        _selectedTab = 0;
        SelectionMode = NavBarItemSelectionMode.Button;

        SetTitle("SettingsViewNavLabel");

        SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);

        GeneralSettingsTab = new GeneralSettingsTabViewModel();
        AdvancedSettingsTab = new AdvancedSettingsTabViewModel();
        ReportSettingsTab = new ReportSettingsViewModel();

        RestartCommand = ReactiveCommand.Create(() =>
            AppLifetimeHelper.Shutdown(withShutdownPrevention: true, restart: true));
        NextCommand = CancelCommand;
    }

    public ICommand RestartCommand
    {
        get;
    }

    public GeneralSettingsTabViewModel GeneralSettingsTab
    {
        get;
    }

    public AdvancedSettingsTabViewModel AdvancedSettingsTab
    {
        get;
    }

    public ReportSettingsViewModel ReportSettingsTab
    {
        get;
    }

    public override MenuViewModel? ToolBar => null;

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;

    private void OnRestartNeeded(
        object? sender
        , RestartNeededEventArgs e
    ) => IsModified = e.IsRestartNeeded;

    protected override void OnNavigatedTo(
        bool isInHistory
        , CompositeDisposable disposables
    )
    {
        base.OnNavigatedTo(isInHistory: isInHistory, disposables: disposables);

        IsModified = SettingsTabViewModelBase.CheckIfRestartIsNeeded();

        SettingsTabViewModelBase.RestartNeeded += OnRestartNeeded;

        disposables.Add(
            Disposable.Create(() => SettingsTabViewModelBase.RestartNeeded -= OnRestartNeeded));
    }
}