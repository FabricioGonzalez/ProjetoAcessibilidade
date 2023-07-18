using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Dialogs;

namespace ProjectAvalonia.Desktop.Extensions;

public static class AppBuilderExtension
{
    public static AppBuilder SetupAppBuilder(
        this AppBuilder appBuilder
    )
    {
        var enableGpu = ServicesConfig.Config is null ? false : ServicesConfig.Config.EnableGpu;

        if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows))
        {
            appBuilder
                .UseWin32()
                .UseSkia();
        }
        else if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Linux))
        {
            appBuilder.UsePlatformDetect()
                .UseManagedSystemDialogs<Window>();
        }
        else
        {
            appBuilder.UsePlatformDetect();
        }

        return appBuilder
            .With(options: new SkiaOptions { MaxGpuResourceSizeBytes = 2560 * 1600 * 4 * 12 })
            .With(options: new Win32PlatformOptions
            {
                OverlayPopups = true, /* AllowEglInitialization = enableGpu, UseDeferredRendering = true,
                UseWindowsUIComposition = true,
                CompositionBackdropCornerRadius = null*/ WinUICompositionBackdropCornerRadius = 4
            })
            .With(options: new X11PlatformOptions
            {
                /*UseGpu = enableGpu,*/ OverlayPopups = true, WmClass = "ProjectAvalonia"
            })
            .With(options: new AvaloniaNativePlatformOptions
                { OverlayPopups = true /* UseDeferredRendering = true, UseGpu = enableGpu*/ })
            .With(options: new MacOSPlatformOptions { ShowInDock = true });
    }
}