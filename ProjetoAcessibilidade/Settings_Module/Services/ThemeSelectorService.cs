using Microsoft.UI.Xaml;
using ProjetoAcessibilidade.Settings_Module.Services.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

namespace ProjetoAcessibilidade.Settings_Module.Services
{
    public class ThemeSelectorService : IThemeSelectorService
    {
        private const string SettingsKey = "AppBackgroundRequestedTheme";
        public ElementTheme Theme { get; set; } = ElementTheme.Light;
        public async Task InitializeAsync()
        {
            Theme = LoadThemeFromSettingsAsync();
            await Task.CompletedTask;
        }
        public async Task SetThemeAsync(ElementTheme theme)
        {
            Theme = theme;

            await SetRequestedThemeAsync();
            SaveThemeInSettingsAsync(Theme);
        }
        public async Task SetRequestedThemeAsync()
        {
            try
            {
                if (App.MainWindow.Content is FrameworkElement rootElement)
                {
                    rootElement.RequestedTheme = Theme;
                }

                await Task.CompletedTask;
            }
            catch (Exception)
            {
                return;
            }
        }
        private ElementTheme LoadThemeFromSettingsAsync()
        {
            ElementTheme cacheTheme = ElementTheme.Default;
            try
            {
                var appData = ApplicationData.Current.LocalSettings;

                if (appData != null)
                {
                    string themeName = (string)appData.Values[SettingsKey];

                    if (!string.IsNullOrEmpty(themeName))
                    {
                        Enum.TryParse(themeName, out cacheTheme);
                    }
                }
                return cacheTheme;
            }
            catch (Exception)
            {

                return cacheTheme;
            }

        }
        private void SaveThemeInSettingsAsync(ElementTheme theme)
        {
            try
            {
                var appData = ApplicationData.Current.LocalSettings;
                if (appData != null)
                {
                    appData.Values[SettingsKey] = theme.ToString();
                }
                return;
            }
            catch (Exception)
            {
                return;
            }

        }
    }
}
