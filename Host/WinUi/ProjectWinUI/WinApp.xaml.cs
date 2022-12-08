// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.UI.Xaml;

using ProjectWinUI.DI;
using ProjectWinUI.Src.Activation.Contracts;
using ProjectWinUI.Src.Helpers;
using ProjectWinUI.Src.Notifications.Contracts;
using ProjectWinUI.Src.Settings.Models;

using WinUIEx;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjectWinUI;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class WinApp : Application
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public WinApp()
    {
        InitializeComponent();

        AppHost = Host.CreateDefaultBuilder().
       UseContentRoot(AppContext.BaseDirectory).
       ConfigureServices((context, services) => {
           services
           .RegisterActivationHandlers()
           .RegisterServices()
           .RegisterViews()
           .RegisterPages()
           .RegisterViewModel()
           .RegisterInfrastructure()
           .RegisterApplication()
           .Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
       })
       .Build();

        /*WinApp.GetService<IAppNotificationService>().Initialize();*/

        UnhandledException += App_UnhandledException;
    }

    public IHost AppHost
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((WinApp.Current as WinApp)!.AppHost.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow
    {
        get;
    } = new MainWindow();
    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        var activatedEventArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();

        if (activatedEventArgs.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.File)
        {
            base.OnLaunched(args);

            await GetService<IActivationService>().ActivateAsync(activatedEventArgs.Data);
            return;
        }

        /*GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));*/
        base.OnLaunched(args);
        await GetService<IActivationService>().ActivateAsync(args);


    }
}
