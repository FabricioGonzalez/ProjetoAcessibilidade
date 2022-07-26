using System.Diagnostics;

using Infrastructure.InMemoryRepository;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using ProjetoAcessibilidade.Activation;
using ProjetoAcessibilidade.Contracts.Services;
using ProjetoAcessibilidade.Core.Contracts.Services;
using ProjetoAcessibilidade.Core.Services;
using ProjetoAcessibilidade.Helpers;
using ProjetoAcessibilidade.Models;
using ProjetoAcessibilidade.Services;
using ProjetoAcessibilidade.ViewModels;
using ProjetoAcessibilidade.Views;

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

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsServicePackaged>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            services.AddSingleton<IFileSelectorService, FileSelectorService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<ProjectViewModel>();
            services.AddTransient<ProjectPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        })
        .Build();

    public static T GetService<T>()
        where T : class
    {
        return _host.Services.GetService(typeof(T)) as T;
    }

    public static Window MainWindow { get; set; } = new Window() { Title = "AppDisplayName".GetLocalized() };

    public App()
    {
        InitializeComponent();
        UnhandledException += App_UnhandledException;
        var appPath = new GetAppLocal().getAppLocal();
        Debug.WriteLine(appPath);
    }

    private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // For more details, see https://docs.microsoft.com/windows/winui/api/microsoft.ui.xaml.unhandledexceptioneventargs.
        try
        {
            Debug.WriteLine(e.Message);
        }
        catch (System.Exception)
        {
        
        
        }
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        var activationService = GetService<IActivationService>();
        await activationService.ActivateAsync(args);
    }
}
