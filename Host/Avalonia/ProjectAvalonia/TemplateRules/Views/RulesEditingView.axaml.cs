using AppViewModels.TemplateRules;

using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using ReactiveUI;

namespace ProjectAvalonia.TemplateRules.Views;
public partial class RulesEditingView : ReactiveUserControl<TemplateRulesViewModel>
{
    public RulesEditingView()
    {
        this.WhenActivated(disposables =>
        {

        });

        AvaloniaXamlLoader.Load(this);
    }
}
