using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Project.AppInstaller.ViewModels;
using Project.AppInstaller.Views;

namespace Project.AppInstaller;

public class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(obj: this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}