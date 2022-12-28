using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Project.AppInstaller.ViewModels;
using Project.AppInstaller.Views;

namespace Project.AppInstaller;
public partial class App : Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
