using System;
using System.Reflection;

using Avalonia;
using Avalonia.Dialogs;
using Avalonia.ReactiveUI;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia;
public class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            // prepare and run your App here
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            // here we can work with the exception, for example add it to our log file
            LogHost.Default.Fatal(e, "Something very bad happened");
        }
    }

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
                .AddQueryHandlers()
                .AddCommandHandlers()
                .AddServices()
                .AddViewModelOperations()
                .AddMediator()
                /*.CreateFolderStructure()*/;


        var result = AppBuilder.Configure<App>()
            .UseManagedSystemDialogs()
                .UsePlatformDetect()
                .LogToTrace()
                .UseSkia()
                .UseReactiveUI();

        return result;
    }
}
