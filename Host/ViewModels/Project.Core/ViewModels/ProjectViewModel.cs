using ReactiveUI;

namespace Project.Core.ViewModels;
public class ProjectViewModel : ViewModelBase, IRoutableViewModel
{

    public IScreen HostScreen
    {
        get;
    }

    public string UrlPathSegment { get; } = "Login";

    public ProjectViewModel(IScreen screen)
    {
        HostScreen = screen;
    }
}
