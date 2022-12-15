using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Neumorphism.Avalonia.Styles.Themes.Base;
using Neumorphism.Avalonia.Styles.Themes;
using Avalonia;

namespace ProjectAvalonia;
public static class GlobalCommand
{
    private static readonly NeumorphismTheme themeStyles = Application.Current!.LocateNeumorphismTheme<NeumorphismTheme>();

    public static void UseNeumorphismUIDarkTheme()
    {
        themeStyles.BaseTheme = BaseThemeMode.Dark;
    }

    public static void UseNeumorphismUILightTheme()
    {
        themeStyles.BaseTheme = BaseThemeMode.Light;
    }
}
