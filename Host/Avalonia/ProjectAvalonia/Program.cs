using System;
using System.Reflection;

using Avalonia;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels.Main;

using ReactiveUI;

using Splat;

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
                .AddUIStates();

        var result = AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
        return result;
    }
}
