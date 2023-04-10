using System;
using System.Linq;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;

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
        private set;
    }

    public static void ApplyTheme(
        Theme theme
    )
    {
        if (Application.Current is not null)
        {
            var currentTheme = Application.Current.Styles.Select(selector: x => (StyleInclude)x)
                .FirstOrDefault(predicate: x =>
                    x.Source is not null && x.Source.AbsolutePath.Contains(value: "Themes"));

            if (currentTheme is not null)
            {
                var themeIndex = Application.Current.Styles.IndexOf(item: currentTheme);

                var newTheme = new StyleInclude(baseUri: new Uri(uriString: "avares://ProjectAvalonia/App.axaml"))
                {
                    Source = new Uri(uriString: $"avares://ProjectAvalonia/Styles/Themes/Base{theme}.axaml")
                };

                CurrentTheme = theme;
                Application.Current.Styles[index: themeIndex] = newTheme;
            }
        }
    }
}