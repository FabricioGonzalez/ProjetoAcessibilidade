using System.IO;

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

namespace ProjetoAcessibilidade.Helpers;
public static class WindowExtension
{
    public static void SetPresenter(this Window window)
    {
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);

        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

        //(appWindow.Presenter as OverlappedPresenter).IsAlwaysOnTop = true;
        //(appWindow.Presenter as OverlappedPresenter).IsModal = true;
        (appWindow.Presenter as OverlappedPresenter).IsMinimizable = false;
        (appWindow.Presenter as OverlappedPresenter).IsMaximizable = false;
        (appWindow.Presenter as OverlappedPresenter).IsResizable = false;
        (appWindow.Presenter as OverlappedPresenter).SetBorderAndTitleBar(true,true);
    }

    public static bool IsTitleBarCustomizable(this Window window)
    {
        if(AppWindowTitleBar.IsCustomizationSupported())
        {
            return true;
        }
        return false;
    }

    //public static void SetWindowIcon(this Window window, string? iconPath = null)
    //{
    //    var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

    //    WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);

    //    AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

    //    iconPath ??= Path.Combine(Package.Current.InstalledPath, "Assets","WindowIcon.ico");

    //    appWindow.SetIcon(iconPath);
    //}
}
