// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using ProjectWinUI.Navigation;
using ProjectWinUI.Src.Helpers;
using ProjectWinUI.Src.Navigation.Contracts;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjectWinUI.Src.Navigation;

/// <summary>
///     An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ShellPage : Page
{
    public ShellPage(
        ShellViewModel viewModel
    )
    {
        ViewModel = viewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(navigationView: NavigationViewControl);

        // TODO: Set the title bar icon by updating /Assets/WindowIcon.ico.
        // A custom title bar is required for full window theme and Mica support.
        // https://docs.microsoft.com/windows/apps/develop/title-bar?tabs=winui3#full-customization
        WinApp.MainWindow.ExtendsContentIntoTitleBar = true;
        WinApp.MainWindow.SetTitleBar(titleBar: AppTitleBar);
        WinApp.MainWindow.Activated += MainWindow_Activated;
        /*AppTitleBarText.Text = "AppDisplayName".GetLocalized();*/
    }

    public ShellViewModel ViewModel
    {
        get;
    }

    private void OnLoaded(
        object sender
        , RoutedEventArgs e
    )
    {
        TitleBarHelper.UpdateTitleBar(theme: RequestedTheme);

        KeyboardAccelerators.Add(item: BuildKeyboardAccelerator(key: VirtualKey.Left
            , modifiers: VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(item: BuildKeyboardAccelerator(key: VirtualKey.GoBack));
    }

    private void MainWindow_Activated(
        object sender
        , WindowActivatedEventArgs args
    )
    {
        var resource = args.WindowActivationState == WindowActivationState.Deactivated
            ? "WindowCaptionForegroundDisabled"
            : "WindowCaptionForeground";

        AppTitleBarText.Foreground = (SolidColorBrush)Application.Current.Resources[key: resource];
    }

    private void NavigationViewControl_DisplayModeChanged(
        NavigationView sender
        , NavigationViewDisplayModeChangedEventArgs args
    ) =>
        AppTitleBar.Margin = new Thickness
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1)
            , Top = AppTitleBar.Margin.Top, Right = AppTitleBar.Margin.Right, Bottom = AppTitleBar.Margin.Bottom
        };

    private static KeyboardAccelerator BuildKeyboardAccelerator(
        VirtualKey key
        , VirtualKeyModifiers? modifiers = null
    )
    {
        var keyboardAccelerator = new KeyboardAccelerator { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(
        KeyboardAccelerator sender
        , KeyboardAcceleratorInvokedEventArgs args
    )
    {
        var navigationService = WinApp.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }
}