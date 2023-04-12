using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.NavBar;

public partial class NavBar : UserControl
{
    public NavBar()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(obj: this);
}