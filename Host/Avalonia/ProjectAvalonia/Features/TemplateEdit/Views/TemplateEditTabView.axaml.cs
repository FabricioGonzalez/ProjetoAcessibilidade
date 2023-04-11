using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.TemplateEdit.Views;

public partial class TemplateEditTabView : UserControl
{
    public TemplateEditTabView()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}