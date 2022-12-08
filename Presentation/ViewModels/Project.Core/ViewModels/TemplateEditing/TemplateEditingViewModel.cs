using Project.Core.ViewModels.Extensions;

using ReactiveUI;

namespace Project.Core.ViewModels.TemplateEditing;
public class TemplateEditingViewModel : ViewModelBase, IRoutableViewModel
{
    public TemplateEditingViewModel()
    {
       
    }

    public string? UrlPathSegment
    {
        get;
    }

    public IScreen HostScreen
    {
        get;set;
    }
}
