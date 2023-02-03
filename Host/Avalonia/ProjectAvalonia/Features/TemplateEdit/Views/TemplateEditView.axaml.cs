using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

using ProjectAvalonia.Features.TemplateEdit.ViewModels;

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

        Dispatcher.UIThread.InvokeAsync(async () => await (DataContext as TemplateEditViewModel).LoadItems());
    }
}
