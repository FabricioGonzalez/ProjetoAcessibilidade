using System.Linq;
using Avalonia;
using Avalonia.Themes.Fluent;

namespace ProjectAvalonia;

public static class GlobalCommand
{
    public static FluentThemeMode GetCurrentTheme()
    {
        var result = Application.Current!.Styles
            .FirstOrDefault(predicate: item => item.GetType() == typeof(FluentTheme));

        return (result as FluentTheme).Mode;
    }

    public static void UseNeumorphismUIDarkTheme()
    {
        var result = Application.Current!.Styles
            .FirstOrDefault(predicate: item => item.GetType() == typeof(FluentTheme));

        if (result is not null)
        {
            (result as FluentTheme).Mode = FluentThemeMode.Dark;
        }
    }

    public static void UseNeumorphismUILightTheme()
    {
        var result = Application.Current!.Styles
            .FirstOrDefault(predicate: item => item.GetType() == typeof(FluentTheme));

        if (result is not null)
        {
            (result as FluentTheme).Mode = FluentThemeMode.Light;
        }
    }
}