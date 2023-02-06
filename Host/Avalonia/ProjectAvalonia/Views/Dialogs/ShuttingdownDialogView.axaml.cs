using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Views.Dialogs;

public partial class ShuttingDownView : UserControl
{
    public ShuttingDownView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
