using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Project.Views.Components.EditingBody;

public partial class SolutionItemView : UserControl
{
    public SolutionItemView()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}