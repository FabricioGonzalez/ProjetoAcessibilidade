using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.WinUI;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

using ProjetoAcessibilidade.Activation;
using ProjetoAcessibilidade.Navigation;
using ProjetoAcessibilidade.Navigation.Contracts;
using ProjetoAcessibilidade.Settings_Module;
using ProjetoAcessibilidade.Settings_Module.Pages;
using ProjetoAcessibilidade.Settings_Module.Services.Contracts;
using ProjetoAcessibilidade.Settings_Module.Services;
using ProjetoAcessibilidade.Settings_Module.ViewModels;

using System;

using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetoAcessibilidade
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static Window MainWindow { get; set; } = new Window() { Title = "AppDisplayName".GetLocalized() };

        public App()
        {
            InitializeComponent();
            UnhandledException += App_UnhandledException;
            Ioc.Default.ConfigureServices(ConfigureServices());

        }

        public static WindowId GetWindowId()
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow);

            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return windowId;
        }

        private void SetWindowIcon()
        {
            AppWindow appWindow = AppWindow.GetFromWindowId(App.GetWindowId());

            appWindow.SetIcon("Resources/app.ico");
        }

        public static bool EnqueueProcess<T>(Action<T> action)
        {
            var result = MainWindow.DispatcherQueue.TryEnqueue(() => action.DynamicInvoke());

            return result;
        }
        public static bool EnqueueProcess(Action action)
        {
            var result = MainWindow.DispatcherQueue.TryEnqueue(() => action.DynamicInvoke());

            return result;
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/windows/winui/api/microsoft.ui.xaml.unhandledexceptioneventargs
            Debug.WriteLine(e.ToString());
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            var activationService = Ioc.Default.GetService<IActivationService>();
            await activationService.ActivateAsync(args);

            SetWindowIcon();    
        }

        private System.IServiceProvider ConfigureServices()
        {
            // TODO WTS: Register your services, viewmodels and pages here
            var services = new ServiceCollection();

            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // DB Context Services

            //services.AddDbContext<SqliteDBContext>();
            //services.AddDbContext<SqlServerDBContext>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();

            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            //services.AddSingleton<UiThreadSyncedProcessor>();

            //services.AddSingleton<MessageDialogService>();

            //services.AddSingleton<PickerService>();

            //services.AddSingleton<RemessaHistoryService>();
            //services.AddSingleton<RetornoHistoryService>();

            //services.AddSingleton<FilterBoletoService>();

            //services.AddSingleton<FileHandlerService>();

            //services.AddSingleton<RelatorioService>();

            //services.AddSingleton<GeradorDeRelatorio>();

            //services.AddSingleton<SyncFolderService>();

            //services.AddSingleton<BaixaService>();

            //services.AddSingleton<ReadXmlService>();

            // Core Services

            // Utility Objects

            //services.AddSingleton<InfoBarObject>();

            //services.AddSingleton<ProgressObject>();

            //services.AddSingleton<MessagerComponent>();
            //services.AddSingleton<ProgressMonitorComponent>();

            //services.AddSingleton<PackageManagerUpdater>();

            //services.AddSingleton(new BoletoInjector(services));
            //services.AddSingleton(new XmlInector(services));

            new SettingsModule().Inject(services);

            // Views and ViewModels
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            //services.AddTransient<ValidaRemessaPage>();
            //services.AddTransient<ValidaRemessaViewModel>();

            services.AddTransient<SettingsPage>();
            services.AddTransient<SettingsViewModel>();

            //services.AddTransient<NFeReaderPage>();
            //services.AddTransient<NFeReaderViewModel>();

            //services.AddTransient<APITestPage>();
            //services.AddTransient<APITestViewModel>();

            //services.AddTransient<RemessaPage>();
            //services.AddTransient<RemessaViewModel>();

            //services.AddTransient<RetornoPage>();
            //services.AddTransient<RetornoViewModel>();

            //services.AddTransient<SyncPage>();
            //services.AddTransient<SyncViewModel>();

            //services.AddTransient<PDFRenderer>();
            //services.AddTransient<PDFRendererViewModel>();


            return services.BuildServiceProvider();
        }
    }
}
