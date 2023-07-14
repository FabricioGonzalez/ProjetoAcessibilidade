using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.ViewModels;
using ProjetoAcessibilidade.Domain.App.Contracts;
using ReactiveUI;

namespace ProjectAvalonia.Features.Settings.ViewModels;

[NavigationMetaData(
    Title = "Opções Gerais",
    Caption = "Gerenciar Opções Gerais",
    Order = 0,
    LocalizedTitle = "GeneralSettingsViewNavLabel",
    Category = "Opções",
    Keywords = new[]
    {
        "Settings", "General", "Bitcoin", "Dark", "Mode", "Run", "Computer", "System", "Start", "Background", "Close"
        , "Auto", "Copy", "Paste", "Addresses", "Custom", "Change", "Address", "Fee", "Display", "Format", "BTC", "sats"
    },
    IconName = "settings_general_regular")]
public partial class GeneralSettingsTabViewModel : SettingsTabViewModelBase
{
    private readonly ILanguageManager languageManager;
    [AutoNotify] private bool _autoCopy;
    [AutoNotify] private bool _autoPaste;
    [AutoNotify] private bool _darkModeEnabled;
    [AutoNotify] private bool _downloadNewVersion;
    [AutoNotify] private bool _hideOnClose;
    [AutoNotify] private AppLanguageModel _language;
    [AutoNotify] private ObservableCollection<AppLanguageModel> _languages;
    [AutoNotify] private bool _runOnSystemStartup;

    public GeneralSettingsTabViewModel()
    {
        SetTitle("GeneralSettingsViewNavLabel");

        Languages = new ObservableCollection<AppLanguageModel>(
            ServicesConfig.LanguageManager.AllLanguages.Select(item =>
                item.ToAppLanguageModel()));

        _darkModeEnabled = ServicesConfig.UiConfig.DarkModeEnabled;
        _autoCopy = ServicesConfig.UiConfig.Autocopy;
        _autoPaste = ServicesConfig.UiConfig.AutoPaste;
        _runOnSystemStartup = ServicesConfig.UiConfig.RunOnSystemStartup;
        _downloadNewVersion = ServicesConfig.UiConfig.RunOnSystemStartup;
        _hideOnClose = ServicesConfig.UiConfig.HideOnClose;
        _language = Languages.FirstOrDefault(i => i.Code == ServicesConfig.Config.AppLanguage)
                    ?? ServicesConfig.LanguageManager.CurrentLanguage.ToAppLanguageModel();

        this.WhenAnyValue(x => x.Language)
            .ObserveOn(RxApp.TaskpoolScheduler)
            .Throttle(TimeSpan.FromMilliseconds(ThrottleTime))
            .Skip(1)
            .Subscribe(prop =>
            {
                ServicesConfig.LanguageManager.SetLanguage(prop.ToLanguageModel());
                ServicesConfig.Config.AppLanguage = prop.Code;

                Save();
            });
        this.WhenAnyValue(x => x.DarkModeEnabled)
            .Skip(1)
            .Subscribe(
                x =>
                {
                    ServicesConfig.UiConfig.DarkModeEnabled = x;
                    Navigate(NavigationTarget.CompactDialogScreen)
                        .To(new ThemeChangeViewModel(x ? Theme.Dark : Theme.Light));
                });

        this.WhenAnyValue(x => x.AutoCopy)
            .ObserveOn(RxApp.TaskpoolScheduler)
            .Skip(1)
            .Subscribe(x => ServicesConfig.UiConfig.Autocopy = x);

        this.WhenAnyValue(x => x.AutoPaste)
            .ObserveOn(RxApp.TaskpoolScheduler)
            .Skip(1)
            .Subscribe(x => ServicesConfig.UiConfig.AutoPaste = x);

        StartupCommand = ReactiveCommand.Create(async () =>
        {
            try
            {
                await StartupHelper.ModifyStartupSettingAsync(RunOnSystemStartup);
                ServicesConfig.UiConfig.RunOnSystemStartup = RunOnSystemStartup;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                RunOnSystemStartup = !RunOnSystemStartup;
                await ShowErrorAsync(Title
                    , "Couldn't save your change, please see the logs for further information.",
                    "Error occurred.");
            }
        });

        this.WhenAnyValue(x => x.HideOnClose)
            .ObserveOn(RxApp.TaskpoolScheduler)
            .Skip(1)
            .Subscribe(x => ServicesConfig.UiConfig.HideOnClose = x);
    }

    public override MenuViewModel? ToolBar => null;

    public ICommand StartupCommand
    {
        get;
    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;

    protected override void EditConfigOnSave(
        Config config
    ) =>
        config.AppLanguage = Language.Code;
}