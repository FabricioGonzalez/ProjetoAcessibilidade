using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Navigation;
using ProjectWinUI.Src.Navigation.Contracts;

namespace ProjectWinUI.Navigation;

public class ShellViewModel : ObservableRecipient
{
    private bool _isBackEnabled;
    private object _selected;

    public ShellViewModel(
        INavigationService navigationService
        , INavigationViewService navigationViewService
    )
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

    public bool IsBackEnabled
    {
        get => _isBackEnabled;
        private set => SetProperty(field: ref _isBackEnabled, newValue: value);
    }

    public object Selected
    {
        get => _selected;
        private set => SetProperty(field: ref _selected, newValue: value);
    }

    private void OnNavigated(
        object sender
        , NavigationEventArgs e
    )
    {
        IsBackEnabled = NavigationService.CanGoBack;

        /*if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }*/

        var selectedItem = NavigationViewService.GetSelectedItem(pageType: e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}