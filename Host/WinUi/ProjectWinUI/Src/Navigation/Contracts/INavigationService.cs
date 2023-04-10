using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace ProjectWinUI.Src.Navigation.Contracts;

public interface INavigationService
{
    bool CanGoBack
    {
        get;
    }

    Frame? Frame
    {
        get;
        set;
    }

    event NavigatedEventHandler Navigated;

    bool NavigateTo(
        string pageKey
        , object? parameter = null
        , bool clearNavigation = false
    );

    bool GoBack();
}