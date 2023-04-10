using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace ProjectWinUI.Src.Theming.Contracts;

public interface IThemeSelectorService
{
    ElementTheme Theme
    {
        get;
    }

    Task InitializeAsync();

    Task SetThemeAsync(
        ElementTheme theme
    );

    Task SetRequestedThemeAsync();
}