using System;
using System.ComponentModel;
using System.Reflection;

using Avalonia;
using Avalonia.ReactiveUI;
using Avalonia.X11;

using MediatR;

using Microsoft.Extensions.Hosting;

using ReactiveUI;

using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace ProjectAvalonia;
internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public IServiceProvider Container
    {
        get; private set;
    }

    void Init()
    {
        var host = Host
          .CreateDefaultBuilder()
          .ConfigureServices(services =>
          {
              services.UseMicrosoftDependencyResolver();
              var resolver = Locator.CurrentMutable;
              resolver.InitializeSplat();
              resolver.InitializeReactiveUI();

              services.AddMediatR(typeof(Program));

          })
          .Build();

        // Since MS DI container is a different type,
        // we need to re-register the built container with Splat again
        Container = host.Services;
        Container.UseMicrosoftDependencyResolver();
    }

    public static AppBuilder BuildAvaloniaApp()
    {


        Locator
            .CurrentMutable
            .RegisterViewsForViewModels(Assembly.GetExecutingAssembly());

        Locator.CurrentMutable
                .AddViewModel()
                .AddViewComponents()
                .AddRepositories()
                .AddUsecases()
                .AddServices()
                .AddUIStates()
                /*.CreateFolderStructure()*/;


        var result = AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

        return result;
    }
}
