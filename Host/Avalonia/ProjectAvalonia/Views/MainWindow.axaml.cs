using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(obj: this);
#if DEBUG
        this.AttachDevTools();
#endif
    }
}