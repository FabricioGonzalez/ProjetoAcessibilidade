using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.TemplateEdit.Views;
public partial class TemplateEditView : UserControl
{
    public TemplateEditView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
