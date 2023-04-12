using System;
using System.Runtime.InteropServices;
using Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using WinRT.Interop;

namespace ProjectWinUI.Src.Helpers;

public class TitleBarHelper
{
    private const int WAINACTIVE = 0x00;
    private const int WAACTIVE = 0x01;
    private const int WMACTIVATE = 0x0006;

    [DllImport(dllName: "user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport(dllName: "user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(
        IntPtr hWnd
        , int msg
        , int wParam
        , IntPtr lParam
    );

    public static void UpdateTitleBar(
        ElementTheme theme
    )
    {
        if (WinApp.MainWindow.ExtendsContentIntoTitleBar)
        {
            if (theme != ElementTheme.Default)
            {
                Application.Current.Resources[key: "WindowCaptionForeground"] = theme switch
                {
                    ElementTheme.Dark => new SolidColorBrush(color: Colors.White)
                    , ElementTheme.Light => new SolidColorBrush(color: Colors.Black)
                    , _ => new SolidColorBrush(color: Colors.Transparent)
                };

                Application.Current.Resources[key: "WindowCaptionForegroundDisabled"] = theme switch
                {
                    ElementTheme.Dark => new SolidColorBrush(color: Color.FromArgb(a: 0x66, r: 0xFF, g: 0xFF, b: 0xFF))
                    , ElementTheme.Light => new SolidColorBrush(color: Color.FromArgb(a: 0x66, r: 0x00, g: 0x00
                        , b: 0x00))
                    , _ => new SolidColorBrush(color: Colors.Transparent)
                };

                Application.Current.Resources[key: "WindowCaptionButtonBackgroundPointerOver"] = theme switch
                {
                    ElementTheme.Dark => new SolidColorBrush(color: Color.FromArgb(a: 0x33, r: 0xFF, g: 0xFF, b: 0xFF))
                    , ElementTheme.Light => new SolidColorBrush(color: Color.FromArgb(a: 0x33, r: 0x00, g: 0x00
                        , b: 0x00))
                    , _ => new SolidColorBrush(color: Colors.Transparent)
                };

                Application.Current.Resources[key: "WindowCaptionButtonBackgroundPressed"] = theme switch
                {
                    ElementTheme.Dark => new SolidColorBrush(color: Color.FromArgb(a: 0x66, r: 0xFF, g: 0xFF, b: 0xFF))
                    , ElementTheme.Light => new SolidColorBrush(color: Color.FromArgb(a: 0x66, r: 0x00, g: 0x00
                        , b: 0x00))
                    , _ => new SolidColorBrush(color: Colors.Transparent)
                };

                Application.Current.Resources[key: "WindowCaptionButtonStrokePointerOver"] = theme switch
                {
                    ElementTheme.Dark => new SolidColorBrush(color: Colors.White)
                    , ElementTheme.Light => new SolidColorBrush(color: Colors.Black)
                    , _ => new SolidColorBrush(color: Colors.Transparent)
                };

                Application.Current.Resources[key: "WindowCaptionButtonStrokePressed"] = theme switch
                {
                    ElementTheme.Dark => new SolidColorBrush(color: Colors.White)
                    , ElementTheme.Light => new SolidColorBrush(color: Colors.Black)
                    , _ => new SolidColorBrush(color: Colors.Transparent)
                };
            }

            Application.Current.Resources[key: "WindowCaptionBackground"] =
                new SolidColorBrush(color: Colors.Transparent);
            Application.Current.Resources[key: "WindowCaptionBackgroundDisabled"] =
                new SolidColorBrush(color: Colors.Transparent);

            var hwnd = WindowNative.GetWindowHandle(target: WinApp.MainWindow);
            if (hwnd == GetActiveWindow())
            {
                SendMessage(hWnd: hwnd, msg: WMACTIVATE, wParam: WAINACTIVE, lParam: IntPtr.Zero);
                SendMessage(hWnd: hwnd, msg: WMACTIVATE, wParam: WAACTIVE, lParam: IntPtr.Zero);
            }
            else
            {
                SendMessage(hWnd: hwnd, msg: WMACTIVATE, wParam: WAACTIVE, lParam: IntPtr.Zero);
                SendMessage(hWnd: hwnd, msg: WMACTIVATE, wParam: WAINACTIVE, lParam: IntPtr.Zero);
            }
        }
    }
}