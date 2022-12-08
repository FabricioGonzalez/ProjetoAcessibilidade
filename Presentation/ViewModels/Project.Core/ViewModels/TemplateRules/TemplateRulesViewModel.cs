using Project.Core.ViewModels.Extensions;

using ReactiveUI;

namespace Project.Core.ViewModels.TemplateEditing;
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
