using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using ProjectWinUI.Src.Helpers;
using ProjectWinUI.Src.Settings.Contracts;
using ProjectWinUI.Src.Theming.Contracts;

namespace ProjectWinUI.Src.Theming.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    private const string SettingsKey = "AppBackgroundRequestedTheme";

    private readonly ILocalSettingsService _localSettingsService;

    public ThemeSelectorService(
        ILocalSettingsService localSettingsService
    )
    {
        _localSettingsService = localSettingsService;
    }

    public ElementTheme Theme
    {
        get;
        set;
    } = ElementTheme.Default;

    public async Task InitializeAsync()
    {
        Theme = await LoadThemeFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetThemeAsync(
        ElementTheme theme
    )
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(theme: Theme);
    }

    public async Task SetRequestedThemeAsync()
    {
        if (WinApp.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(theme: Theme);
        }

        await Task.CompletedTask;
    }

    private async Task<ElementTheme> LoadThemeFromSettingsAsync()
    {
        var themeName = await _localSettingsService.ReadSettingAsync<string>(key: SettingsKey);

        if (Enum.TryParse(value: themeName, result: out ElementTheme cacheTheme))
        {
            return cacheTheme;
        }

        return ElementTheme.Default;
    }

    private async Task SaveThemeInSettingsAsync(
        ElementTheme theme
    ) => await _localSettingsService.SaveSettingAsync(key: SettingsKey, value: theme.ToString());
}