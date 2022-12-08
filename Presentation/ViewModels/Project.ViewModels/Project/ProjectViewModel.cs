using AppViewModels.Common;

using ReactiveUI;

namespace AppViewModels.Project;
public class ProjectViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen
    {
        get;set;
    }
    public string UrlPathSegment { get; } = "Project";
    public string strFolder
    {
        get; set;
    } = "";



    public ProjectViewModel()
    {
    }
}
