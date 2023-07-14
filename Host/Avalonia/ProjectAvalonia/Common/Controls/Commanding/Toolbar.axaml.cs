using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Common.Controls.Commanding;

public partial class ToolBar : UserControl
{
    public ToolBar()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}