using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Project.Views.Components.SolutionBody;

public partial class ClientSection : UserControl
{
    public ClientSection()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {

        AvaloniaXamlLoader.Load(this);
    }
}