using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectAvalonia.ViewModels;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace ProjectAvalonia;

public class App : Application
{
    private readonly Func<Task>? _backendInitialiseAsync;
    private readonly bool _startInBg;
    private ApplicationStateManager? _applicationStateManager;

    public App()
    {
        Name = Constants.AppName;
    }

    public App(
        Func<Task> backendInitialiseAsync
        , bool startInBg
    ) : this()
    {
        _startInBg = startInBg;
        _backendInitialiseAsync = backendInitialiseAsync;
    }

    public IServiceProvider Container
    {
        get;
        private set;
    }

    public IHost host
    {
        get;
        set;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Init();
    }

    public void Init()
    {
        host = Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.UseMicrosoftDependencyResolver();
                var resolver = Locator.CurrentMutable;
                resolver.InitializeSplat();
                resolver.InitializeReactiveUI();

                resolver.RegisterConstant(value: new AvaloniaActivationForViewFetcher()
                    , serviceType: typeof(IActivationForViewFetcher));
                resolver.RegisterConstant(value: new AutoDataTemplateBindingHook()
                    , serviceType: typeof(IPropertyBindingHook));
                RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;

                // Configure our local services and access the host configuration
                ConfigureServices(services);
            })
            /*.ConfigureLogging(loggingBuilder =>
            {
                /*
                //remove loggers incompatible with UWP
                {
                  var eventLoggers = loggingBuilder.Services
                    .Where(l => l.ImplementationType == typeof(EventLogLoggerProvider))
                    .ToList();

                  foreach (var el in eventLoggers)
                    loggingBuilder.Services.Remove(el);
                }
                #1#

                loggingBuilder.AddSplat();
            })*/
            .UseEnvironment(Environments.Development)
            .Build();

        // Since MS DI container is a different type,
        // we need to re-register the built container with Splat again
        Container = host.Services;
        Container.UseMicrosoftDependencyResolver();


        RxApp.DefaultExceptionHandler = Observer.Create<Exception>(
            ex =>
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                Debug.WriteLine(ex);

                RxApp.MainThreadScheduler.Schedule(
                    () => throw new ApplicationException(
                        message: "Exception has been thrown in unobserved ThrownExceptions",
                        innerException: ex));
            });
    }

    private void ConfigureServices(
        IServiceCollection services
    )
    {
    }

    public override void OnFrameworkInitializationCompleted()
    {
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
        if (!Design.IsDesignMode)
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                _applicationStateManager =
                    new ApplicationStateManager(lifetime: desktop, startInBg: _startInBg);

                DataContext = _applicationStateManager.ApplicationViewModel;

                desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                RxApp.MainThreadScheduler.Schedule(
                    action: async () =>
                    {
                        await _backendInitialiseAsync!(); // Guaranteed not to be null when not in designer.

                        MainViewModel.Instance.Initialize();
                    });
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}