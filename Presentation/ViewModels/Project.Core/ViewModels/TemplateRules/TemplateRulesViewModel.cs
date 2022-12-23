using Project.Core.ViewModels.Extensions;

using ReactiveUI;

namespace Project.Core.ViewModels.TemplateRules;
public class TemplateRulesViewModel : ViewModelBase, IRoutableViewModel
{
    public TemplateRulesViewModel()
    {
    }

    public string? UrlPathSegment
    {
        get;
    }

    public IScreen HostScreen
    {
        get; set;
    }
}
