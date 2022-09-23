﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AppWinui.AppCode.AppUtils.Helpers;

// Helper class to set the navigation target for a NavigationViewItem.
//
// Usage in XAML:
// <NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavigationHelper.NavigateTo="AppName.ViewModels.MainViewModel" />
//
// Usage in code:
// NavigationHelper.SetNavigateTo(navigationViewItem, typeof(MainViewModel).FullName);
public class NavigationHelper
{
    public static string GetNavigateTo(NavigationViewItem item) => (string)item.GetValue(NavigateToProperty);

    public static void SetNavigateTo(NavigationViewItem item, string value) => item.SetValue(NavigateToProperty, value); 

    public static readonly DependencyProperty NavigateToProperty =
        DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationHelper), new PropertyMetadata(null)); 
    
    public static object GetParameter(NavigationViewItem item)
        => (object)item.GetValue(NavigateToParameterProperty);

    public static void SetParameter(NavigationViewItem item, object value)
        => item.SetValue(NavigateToParameterProperty, value);
    public static readonly DependencyProperty NavigateToParameterProperty =
        DependencyProperty.RegisterAttached(
            "NavigationParameter",
            typeof(object), 
            typeof(NavigationHelper), 
            new PropertyMetadata(null));
}
