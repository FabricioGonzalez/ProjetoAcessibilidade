using Avalonia.Styling;

namespace ProjectAvalonia.Common.Helpers;

public enum Theme
{
    Dark
    , Light
}

public static class ThemeHelper
{
    public static Theme CurrentTheme
    {
        get;
    }

    public static void ApplyTheme(
        Theme theme
    )
    {
        if (Avalonia.Application.Current is not null)
        {
            var requestedTheme = theme switch
            {
                Theme.Light => ThemeVariant.Light, Theme.Dark => ThemeVariant.Dark, _ => ThemeVariant.Default
            };
            Avalonia.Application.Current.RequestedThemeVariant = requestedTheme;
            /*var currentTheme = Domain.Application.Current.Styles.Select(selector: x => (StyleInclude)x)
                .FirstOrDefault(predicate: x =>
                    x.Source is not null && x.Source.AbsolutePath.Contains(value: "Themes"));

            if (currentTheme is not null)
            {
                var themeIndex = Domain.Application.Current.Styles.IndexOf(item: currentTheme);

                var newTheme = new StyleInclude(baseUri: new Uri(uriString: "avares://ProjectAvalonia/App.axaml"))
                {
                    Source = new Uri(uriString: $"avares://ProjectAvalonia/Styles/Themes/Base{theme}.axaml")
                };

                CurrentTheme = theme;
                Domain.Application.Current.Styles[index: themeIndex] = newTheme;*/
        }
    }
}