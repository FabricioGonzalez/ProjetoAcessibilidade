using Microsoft.Extensions.DependencyInjection;

using Core.Contracts;
using ProjetoAcessibilidade.Settings_Module.Services;
using ProjetoAcessibilidade.Settings_Module.Pages;
using ProjetoAcessibilidade.Settings_Module.ViewModels;
using ProjetoAcessibilidade.Settings_Module.Services.Contracts;

namespace ProjetoAcessibilidade.Settings_Module
{
    public class SettingsModule : IAppModule
    {
        public void Inject(ServiceCollection serviceProvider)
        {
   
            serviceProvider.AddSingleton<SettingsSplitViewService>();       
            
            serviceProvider.AddTransient<SettingsThemeViewModel>();
            serviceProvider.AddTransient<SettingsThemePage>();
            
            ConfigureSettingsPage();
        }

        private void ConfigureSettingsPage()
        {
            SettingsSplitViewService.RegisterPages<SettingsThemePage, SettingsThemeViewModel>();
        }
    }
}
