using AppUsecases;
using AppUsecases.Contracts.Repositories;
using AppUsecases.Contracts.Services;
using AppUsecases.Contracts.Usecases;
using AppUsecases.Entities.FileTemplate;
using AppUsecases.Usecases;

using AppWinui.Activation;
using AppWinui.AppCode.AppUtils.Contracts.Services;
using AppWinui.AppCode.AppUtils.Helpers;
using AppWinui.AppCode.AppUtils.Services;
using AppWinui.AppCode.AppUtils.Services.Services;
using AppWinui.AppCode.AppUtils.UIModels.Models;
using AppWinui.AppCode.AppUtils.ViewModels;
using AppWinui.AppCode.AppUtils.Views;
using AppWinui.AppCode.Home.ViewModels;
using AppWinui.AppCode.Home.Views;
using AppWinui.AppCode.Project.ViewModels;
using AppWinui.AppCode.Project.Views;
using AppWinui.AppCode.TemplateEditing.ViewModels;
using AppWinui.AppCode.TemplateEditing.Views;
using AppWinui.Core.Contracts.Services;
using AppWinui.Core.Services;

using LocalRepository;
using LocalRepository.FileRepository.Repository.InternalAppFiles;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace AppWinui;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<IFilePickerService, FilePickerService>();
            services.AddSingleton<IFolderPickerService, FolderPickerService>();

            RepositoryInjector.Inject(services);
            ApplicationInjector.Inject(services);

            // Core Services
            services.AddSingleton<ISampleDataService, SampleDataService>();
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<ApplicationViewModel>();

            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            services.AddTransient<TemplateEditViewModel>();
            services.AddTransient<ListDetailsPage>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();

            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            services.AddTransient<ProjectPage>();
            services.AddTransient<ProjectViewModel>();

            services.AddTransient<ExplorerViewViewModel>();
            services.AddTransient<RecentOpenedViewModel>();
            services.AddTransient<ProjectItemViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        App.GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        var activatedEventArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();

        if (activatedEventArgs.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.File)
        {
            base.OnLaunched(args);

            await GetService<IActivationService>().ActivateAsync(activatedEventArgs.Data);
            return;
        }

        GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));
        base.OnLaunched(args);
        await GetService<IActivationService>().ActivateAsync(args);


    }
}
