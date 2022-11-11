using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Project.Core.ViewModels;
using Project.Core.ViewModels.Main;

using ProjectAvalonia.Views;

using Splat;

namespace ProjectAvalonia;
public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var args = desktop.Args;
            var window = Locator.Current.GetService<MainWindow>();

            var windowViewModel = Locator.Current.GetService<MainWindowViewModel>();

            if (args is not null)
                windowViewModel.SetProjectPath(args.ToString());

            window.DataContext = windowViewModel;

            desktop.MainWindow = window;
        }

        base.OnFrameworkInitializationCompleted();
    }
}