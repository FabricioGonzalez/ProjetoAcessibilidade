using System;
using System.Reflection;

using Avalonia;
using Avalonia.Dialogs;
using Avalonia.ReactiveUI;

using ProjectAvalonia.Views;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia;
internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            /* TODO Há um Bug ao mudar o tema onde o collection binding do UFList da erro*/

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
                .AddUsecases()
                .AddServices()
                .AddUIStates()
                .AddViewModelOperations()
                .AddMediator()
                /*.CreateFolderStructure()*/;


        var result = AppBuilder.Configure<App>()
            .UseManagedSystemDialogs()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

        return result;
    }
}
