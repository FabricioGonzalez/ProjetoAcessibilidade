using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProjectAvalonia.Common.Helpers;

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

    protected override void OnLoaded(
        RoutedEventArgs e
    ) =>
        NotificationHelpers.SetNotificationManager(this);
}