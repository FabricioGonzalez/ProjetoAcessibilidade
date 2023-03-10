using System.Windows.Input;

using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.ViewModels;

namespace ProjectAvalonia.Features.Project.ViewModels;
public partial class ProjectExplorerViewModel : ViewModelBase
{
    [AutoNotify] private SolutionStateViewModel _solutionModel;
    [AutoNotify] private bool _isDocumentSolutionEnabled = false;

    [AutoNotify] private ItemState _selectedItem;

    public ProjectExplorerViewModel()
    {

    }
    public ICommand CreateItemCommand
    {
        get; set;
    }

    public ICommand PrintProjectCommand
    {
        get; set;
    }
    public ICommand OpenSolutionCommand
    {

        get; set;
    }
    public ICommand SaveSolutionCommand
    {
        get; set;
    }
}
