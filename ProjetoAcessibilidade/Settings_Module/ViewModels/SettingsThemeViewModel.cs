using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;

using ProjetoAcessibilidade.Settings_Module.Services.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Windows.ApplicationModel;

namespace ProjetoAcessibilidade.Settings_Module.ViewModels
{
     public class SettingsThemeViewModel : ObservableRecipient
    {

        /*MessagerComponent messager = Ioc.Default.GetService<MessagerComponent>();
        public InfoBarObject infoBarObject { get; } = Ioc.Default.GetService<InfoBarObject>();*/

        private bool hasUpdates = false;
        public bool HasUpdates
        {
            get { return hasUpdates = false; }
            set
            {
                SetProperty(ref hasUpdates, value);
                OnPropertyChanged();
            }
        }

        private readonly IThemeSelectorService _themeSelectorService;

        private ElementTheme _elementTheme;
        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { SetProperty(ref _elementTheme, value); }
        }

        private string _versionDescription;
        public string VersionDescription
        {
            get { return _versionDescription; }

            set { SetProperty(ref _versionDescription, value); }
        }

        private ICommand _switchThemeCommand;
        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            if (ElementTheme != param)
                            {
                                ElementTheme = param;
                                await _themeSelectorService.SetThemeAsync(param);
                            }
                        });
                }

                return _switchThemeCommand;
            }
        }

        public SettingsThemeViewModel(IThemeSelectorService themeSelectorService)
        {
            _themeSelectorService = themeSelectorService;
            _elementTheme = _themeSelectorService.Theme;
            VersionDescription = GetVersionDescription();
            //messager.Attach(infoBarObject);
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();

            Package package = Package.Current;
            PackageVersion packageVersion = package.Id.Version;

            var currentVersion = new Version(string.Format("{0}.{1}.{2}.{3}",
                packageVersion.Major,
                packageVersion.Minor,
                packageVersion.Build,
                packageVersion.Revision));

            return $"{appName} - {currentVersion}";
        }

        private ICommand _checkUpdatedCommand;
   
        /* public ICommand CheckUpdatedCommand
        {
            get
            {
                if (_checkUpdatedCommand == null)
                {
                    _checkUpdatedCommand = new RelayCommand(async () =>
                    {
                        var packageManager = Ioc.Default.GetService<PackageManagerUpdater>();
                        var result = await packageManager.CheckUpdate();

                        if (result)
                        {
                            messager.SetValue("Atualização Encontrada!",
                                "Deseja Atualizar?",
                                true,
                                InfoBarSeverity.Informational,
                                true,
                                "Atualizar",
                                new RelayCommand(() =>
                                {
                                    packageManager.CommandInvokedHandler();
                                }),
                                null,
                                false);
                        }
                        else
                        {
                            messager.SetValue("Sem Atualizações!",
                              "Nenhuma atualização encontrada \n",
                              true,
                              InfoBarSeverity.Informational,
                              false);
                        }
                    });
                }

                return _checkUpdatedCommand;
            }
        }*/
    }
}
