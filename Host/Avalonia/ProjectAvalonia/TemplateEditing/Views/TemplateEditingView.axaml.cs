using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels.TemplateEditing;

using ReactiveUI;

namespace ProjectAvalonia.TemplateEditing.Views;
public partial class TemplateEditingView : ReactiveUserControl<TemplateEditingViewModel>
{
    public TemplateEditingView()
    {
        this.WhenActivated(disposables =>
        {

        });

        AvaloniaXamlLoader.Load(this);
    }
}
