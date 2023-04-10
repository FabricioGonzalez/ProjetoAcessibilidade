// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using ProjectWinUI.DI;
using ProjectWinUI.Src.Activation.Contracts;
using ProjectWinUI.Src.Settings.Models;
using WinUIEx;
using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjectWinUI;

/// <summary>
///     Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class WinApp : Application
{
    /// <summary>
    ///     Initializes the singleton application object.  This is the first line of authored code
    ///     executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public WinApp()
    {
        InitializeComponent();

        AppHost = Host.CreateDefaultBuilder().UseContentRoot(contentRoot: AppContext.BaseDirectory).ConfigureServices(
                configureDelegate: (
                    context
                    , services
                ) =>
                {
                    services
                        .RegisterActivationHandlers()
                        .RegisterServices()
                        .RegisterViews()
                        .RegisterPages()
                        .RegisterViewModel()
                        .RegisterInfrastructure()
                        .RegisterApplication()
                        .Configure<LocalSettingsOptions>(
                            config: context.Configuration.GetSection(key: nameof(LocalSettingsOptions)));
                })
            .Build();

        /*WinApp.GetService<IAppNotificationService>().Initialize();*/

        UnhandledException += App_UnhandledException;
    }

    public IHost AppHost
    {
        get;
    }

    public static WindowEx MainWindow
    {
        get;
    } = new MainWindow();

    public static T GetService<T>()
        where T : class
    {
        if ((Current as WinApp)!.AppHost.Services.GetService(serviceType: typeof(T)) is not T service)
        {
            throw new ArgumentException(
                message: $"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    private void App_UnhandledException(
        object sender
        , UnhandledExceptionEventArgs e
    )
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(
        LaunchActivatedEventArgs args
    )
    {
        base.OnLaunched(args: args);

        var activatedEventArgs = AppInstance.GetCurrent().GetActivatedEventArgs();

        if (activatedEventArgs.Kind == ExtendedActivationKind.File)
        {
            base.OnLaunched(args: args);

            await GetService<IActivationService>().ActivateAsync(activationArgs: activatedEventArgs.Data);
            return;
        }

        /*GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));*/
        base.OnLaunched(args: args);
        await GetService<IActivationService>().ActivateAsync(activationArgs: args);
    }
}