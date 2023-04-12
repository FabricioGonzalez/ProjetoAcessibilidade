using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;

namespace ProjectWinUI.Src.Navigation.Contracts;

public interface INavigationViewService
{
    IList<object>? MenuItems
    {
        get;
    }

    object? SettingsItem
    {
        get;
    }

    void Initialize(
        NavigationView navigationView
    );

    void UnregisterEvents();

    NavigationViewItem? GetSelectedItem(
        Type pageType
    );
}