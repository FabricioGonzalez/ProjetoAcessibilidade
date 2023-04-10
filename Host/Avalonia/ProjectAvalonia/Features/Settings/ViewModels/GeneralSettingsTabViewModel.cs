using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Project.Domain.App.Contracts;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Logging;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Settings.ViewModels;

[NavigationMetaData(
    Title = "Opções Gerais",
    Caption = "Gerenciar Opções Gerais",
    Order = 0,
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
        languageManager = Locator.Current.GetService<ILanguageManager>();

        _darkModeEnabled = ServicesConfig.UiConfig.DarkModeEnabled;
        _autoCopy = ServicesConfig.UiConfig.Autocopy;
        _autoPaste = ServicesConfig.UiConfig.AutoPaste;
        _runOnSystemStartup = ServicesConfig.UiConfig.RunOnSystemStartup;
        _downloadNewVersion = ServicesConfig.UiConfig.RunOnSystemStartup;
        _hideOnClose = ServicesConfig.UiConfig.HideOnClose;

        Languages = new ObservableCollection<AppLanguageModel>(
            collection: languageManager.AllLanguages.Select(selector: item => item.ToAppLanguageModel()));

        Language = Languages.FirstOrDefault(predicate: i => i.Code == ServicesConfig.Config.AppLanguage)
                   ?? languageManager.CurrentLanguage.ToAppLanguageModel();

        this.WhenAnyValue(property1: vm => vm.Language)
            .WhereNotNull()
            .Subscribe(onNext: prop =>
            {
                languageManager.SetLanguage(languageModel: prop.ToLanguageModel());
                ServicesConfig.Config.AppLanguage = prop.Code;
            });

        this.WhenAnyValue(property1: x => x.DarkModeEnabled)
            .Skip(count: 1)
            .Subscribe(
                onNext: x =>
                {
                    ServicesConfig.UiConfig.DarkModeEnabled = x;
                    Navigate(currentTarget: NavigationTarget.CompactDialogScreen)
                        .To(viewmodel: new ThemeChangeViewModel(newTheme: x ? Theme.Dark : Theme.Light));
                });

        this.WhenAnyValue(property1: x => x.AutoCopy)
            .ObserveOn(scheduler: RxApp.TaskpoolScheduler)
            .Skip(count: 1)
            .Subscribe(onNext: x => ServicesConfig.UiConfig.Autocopy = x);

        this.WhenAnyValue(property1: x => x.AutoPaste)
            .ObserveOn(scheduler: RxApp.TaskpoolScheduler)
            .Skip(count: 1)
            .Subscribe(onNext: x => ServicesConfig.UiConfig.AutoPaste = x);

        StartupCommand = ReactiveCommand.Create(execute: async () =>
        {
            try
            {
                await StartupHelper.ModifyStartupSettingAsync(runOnSystemStartup: RunOnSystemStartup);
                ServicesConfig.UiConfig.RunOnSystemStartup = RunOnSystemStartup;
            }
            catch (Exception ex)
            {
                Logger.LogError(exception: ex);
                RunOnSystemStartup = !RunOnSystemStartup;
                await ShowErrorAsync(title: Title
                    , message: "Couldn't save your change, please see the logs for further information.",
                    caption: "Error occurred.");
            }
        });

        this.WhenAnyValue(property1: x => x.HideOnClose)
            .ObserveOn(scheduler: RxApp.TaskpoolScheduler)
            .Skip(count: 1)
            .Subscribe(onNext: x => ServicesConfig.UiConfig.HideOnClose = x);
    }

    public ICommand StartupCommand
    {
        get;
    }

    protected override void EditConfigOnSave(
        Config config
    )
    {
    }
}