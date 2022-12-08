using Project.Core.ViewModels.Extensions;
using ReactiveUI;

namespace Project.Core.ViewModels.Project;
public class ProjectViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen
    {
        get;set;
    }

    public string strFolder
    {
        get; set;
    } = "";

    public string UrlPathSegment { get; } = "Project";



    public ProjectViewModel()
    {
    }
}
