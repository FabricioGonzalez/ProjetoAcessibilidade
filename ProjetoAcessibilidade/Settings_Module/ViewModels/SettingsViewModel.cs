using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Core.Types;

using ProjetoAcessibilidade.Settings_Module.Services;

using System.Collections.Generic;
using System.Windows.Input;

namespace ProjetoAcessibilidade.Settings_Module.ViewModels
{
    public class SettingsViewModel : ObservableRecipient
    {
        SettingsSplitViewService settingsSplitViewService;

        private bool paneOpen = true;
        public bool PaneOpen
        {
            get { return paneOpen; }
            set
            {
                SetProperty(ref paneOpen, value);
                OnPropertyChanged();
            }
        }

        private int selectedItem = -1;
        public int SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));

                if (SelectedItem > -1)
                    settingsSplitViewService.SetContent(Components[value].element);
            }
        }
        public List<ControlInstance> Components { get; set; } = new List<ControlInstance>() {
        new ControlInstance()
        {
            Name="Configurações de Tema",
            element = typeof(SettingsThemeViewModel),
        },
        //new ControlInstance()
        //{
        //    Name="Configurações do Sistema",
        //    element = typeof(SystemSettingsViewModel)
        //}

        };
        public ICommand OpenPane { get; }
        public SettingsViewModel(SettingsSplitViewService settingsSplitViewService)
        {
            this.settingsSplitViewService = settingsSplitViewService;
            
            OpenPane = new RelayCommand(() =>
            {
                PaneOpen = !PaneOpen;
            });
        }
    }
}
