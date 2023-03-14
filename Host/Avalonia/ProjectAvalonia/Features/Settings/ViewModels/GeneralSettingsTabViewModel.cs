using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

using Project.Domain.App.Contracts;

using ProjectAvalonia.Common.Extensions;
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
            "Settings", "General", "Bitcoin", "Dark", "Mode", "Run", "Computer", "System", "Start", "Background", "Close",
            "Auto", "Copy", "Paste", "Addresses", "Custom", "Change", "Address", "Fee", "Display", "Format", "BTC", "sats"
    },
    IconName = "settings_general_regular")]
public partial class GeneralSettingsTabViewModel : SettingsTabViewModelBase
{
    [AutoNotify] private bool _darkModeEnabled;
    [AutoNotify] private bool _autoCopy;
    [AutoNotify] private bool _autoPaste;
    [AutoNotify] private bool _runOnSystemStartup;
    [AutoNotify] private bool _hideOnClose;
    [AutoNotify] private bool _downloadNewVersion;
    [AutoNotify] private ObservableCollection<AppLanguageModel> _languages;
    [AutoNotify] private AppLanguageModel _language;

    public GeneralSettingsTabViewModel()
    {
        _darkModeEnabled = ServicesConfig.UiConfig.DarkModeEnabled;
        _autoCopy = ServicesConfig.UiConfig.Autocopy;
        _autoPaste = ServicesConfig.UiConfig.AutoPaste;
        _runOnSystemStartup = ServicesConfig.UiConfig.RunOnSystemStartup;
        _downloadNewVersion = ServicesConfig.UiConfig.RunOnSystemStartup;
        _hideOnClose = ServicesConfig.UiConfig.HideOnClose;

        var languageManager = Locator.Current.GetService<ILanguageManager>();

        Languages = new ObservableCollection<AppLanguageModel>(languageManager.AllLanguages.Select(item => item.ToAppLanguageModel()));
        Language = languageManager.CurrentLanguage.ToAppLanguageModel();


        this.WhenAnyValue(vm => vm.Language)
            .WhereNotNull()
            .Subscribe((prop) =>
            {
                languageManager.SetLanguage(prop.ToLanguageModel());

                var result = "ThemeChangingLabel".GetLocalized();

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
                await ShowErrorAsync(Title, "Couldn't save your change, please see the logs for further information.",
                    "Error occurred.");
            }
        });

        this.WhenAnyValue(x => x.HideOnClose)
            .ObserveOn(RxApp.TaskpoolScheduler)
            .Skip(1)
            .Subscribe(x => ServicesConfig.UiConfig.HideOnClose = x);
    }

    public ICommand StartupCommand
    {
        get;
    }
    protected override void EditConfigOnSave(Config config)
    {
    }
}
