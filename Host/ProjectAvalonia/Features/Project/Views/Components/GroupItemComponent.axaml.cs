using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Project.Views.Components;

public partial class GroupItemComponent : UserControl
{
    public GroupItemComponent()
    {
        InitializeComponent();
    }
    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}