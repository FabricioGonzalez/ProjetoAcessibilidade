using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.TemplateEdit.Components;

public partial class CbEditContainer : UserControl
{
    public CbEditContainer()
    {
        InitializeComponent();
    }

    public void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}