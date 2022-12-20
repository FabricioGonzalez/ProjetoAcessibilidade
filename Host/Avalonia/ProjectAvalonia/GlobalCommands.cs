using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Neumorphism.Avalonia.Styles.Themes.Base;
using Neumorphism.Avalonia.Styles.Themes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Themes.Fluent;

namespace ProjectAvalonia;
public static class GlobalCommand
{
    public static FluentThemeMode GetCurrentTheme()
    {
        var result = Application.Current!.Styles
        .FirstOrDefault(item => item.GetType() == typeof(FluentTheme));

        return (result as FluentTheme).Mode;
    }

    public static void UseNeumorphismUIDarkTheme()
    {
        var result = Application.Current!.Styles
        .FirstOrDefault(item => item.GetType() == typeof(FluentTheme));

        if (result is not null)
        {
            (result as FluentTheme).Mode = FluentThemeMode.Dark;
        }
    }

    public static void UseNeumorphismUILightTheme()
    {
        var result = Application.Current!.Styles
             .FirstOrDefault(item => item.GetType() == typeof(FluentTheme));

        if (result is not null)
        {
            (result as FluentTheme).Mode = FluentThemeMode.Light;
        }
    }
}
