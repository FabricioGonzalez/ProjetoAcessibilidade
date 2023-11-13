using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.TemplateEdit.Components;

public partial class TextContainer : UserControl
{
    public TextContainer()
    {
        InitializeComponent();
    }

    public void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}