using System.Reactive.Disposables;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels;

using ReactiveUI;

namespace ProjectAvalonia.Views;
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposable =>
        {
            Disposable.Create(() => { }).DisposeWith(disposable);
        });

        AvaloniaXamlLoader.Load(this);
#if DEBUG
        this.AttachDevTools();
#endif
    }
}