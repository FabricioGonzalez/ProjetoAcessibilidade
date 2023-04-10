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
        AvaloniaXamlLoader.Load(obj: this);

        Dispatcher.UIThread.InvokeAsync(function: async () =>
        {
            if (DataContext is TemplateEditViewModel vm)
            {
                await vm.LoadItems();
            }
        });
    }
}