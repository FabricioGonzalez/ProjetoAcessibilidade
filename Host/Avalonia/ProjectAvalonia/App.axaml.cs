using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Project.Core.ViewModels;

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
            desktop.MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}