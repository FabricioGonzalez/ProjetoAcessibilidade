using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Infrastructure.InMemoryRepository;
using Infrastructure.WindowsStorageRepository;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

using ProjetoAcessibilidade.Activation;
using ProjetoAcessibilidade.Contracts.Services;
using ProjetoAcessibilidade.Core.Contracts.Services;
using ProjetoAcessibilidade.Core.Services;
using ProjetoAcessibilidade.Helpers;
using ProjetoAcessibilidade.Models;
using ProjetoAcessibilidade.Services;
using ProjetoAcessibilidade.ViewModels;
using ProjetoAcessibilidade.ViewModels.DialogViewModel;
using ProjetoAcessibilidade.Views;
using ProjetoAcessibilidade.Views.Dialogs;

using SystemApplication.Services.Contracts;
using SystemApplication.Services.ProjectDataServices;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;
using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;

// To learn more about WinUI3, see: https://docs.microsoft.com/windows/apps/winui/winui3/.
namespace ProjetoAcessibilidade;

public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    private static readonly IHost _host = Host
        .CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            services.TryAddTransient<IActivationHandler, FileActivationHandler>();

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsServicePackaged>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            services.AddSingleton<NewItemDialogService>();

            services.AddSingleton<InfoBarService>();

            services.AddScoped<GetAppLocal>((services) => new(Path.Combine(Package.Current.InstalledPath, "Specifications")));

            services.AddScoped<IProjectSolutionRepository, ProjectSolutionRepository>((services) => new(Path.Combine(Package.Current.InstalledPath, "Specifications")));

            services.AddSingleton<IFileSelectorService, FileSelectorService>();
            services.AddSingleton<IXmlProjectDataRepository, XmlProjectDataRepository>();

            services.AddScoped<GetProjectData>();
            services.AddScoped<CreateProjectData>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            services.AddTransient<ProjectViewModel>();
            services.AddTransient<ProjectPage>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();

            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            services.AddTransient<TemplateEditingPage>();
            services.AddTransient<TemplateEditViewModel>();

            services.AddTransient<PrintPage>();
            services.AddTransient<PrintViewModel>();

            services.AddTransient<NewItemDialog>();
            services.AddSingleton<NewItemViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        })
        .Build();
    private WindowId GetWindowId()
    {
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow);

        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return windowId;
    }

    private void SetWindowIcon()
    {
        AppWindow appWindow = AppWindow.GetFromWindowId(GetWindowId());

        appWindow.SetIcon(Path.Combine(Package.Current.InstalledLocation.Path, "WindowIcon.ico"));
    }

    public static T GetService<T>()
        where T : class
    {
        return _host.Services.GetService(typeof(T)) as T;
    }
    public static object GetService(Type type)
    {
        return _host.Services.GetService(type) as object;
    }

    public static Window MainWindow { get; set; } = new Window() { Title = "AppDisplayName".GetLocalized() };

    public App()
    {
        InitializeComponent();
        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // For more details, see https://docs.microsoft.com/windows/winui/api/microsoft.ui.xaml.unhandledexceptioneventargs.
        try
        {
            var r = App.GetService<InfoBarService>();
            r.SetMessageData("Erro", e.ToString(), Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
        }
        catch (System.Exception)
        {
            return;
        }
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {

        var activationService = GetService<IActivationService>();

        var activatedEventArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();

        if (activatedEventArgs.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.File)
        {
            base.OnLaunched(args);

            await activationService.ActivateAsync(activatedEventArgs.Data);
        }
        else
        {
            base.OnLaunched(args);
            await activationService.ActivateAsync(args);
        }
        if (!MainWindow.IsTitleBarCustomizable())
        {
            SetWindowIcon();
        }
    }
}
