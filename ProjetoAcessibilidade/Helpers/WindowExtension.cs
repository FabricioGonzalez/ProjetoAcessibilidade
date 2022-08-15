using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace ProjetoAcessibilidade.Helpers;
public static class WindowExtension
{
    public static void SetPresenter(this Window window)
    {
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);

        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

        var presenter = CompactOverlayPresenter.Create();

        appWindow.SetPresenter(presenter);
    }
    public static void SetWindowIcon(this Window window, string iconPath = "Resources/WindowIcon.png")
    {
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);

        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

        appWindow.SetIcon(iconPath);
    }
}
