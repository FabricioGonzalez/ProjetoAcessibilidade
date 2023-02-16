using System.Windows.Input;

using Core.Entities.Solution.ItemsGroup;

using ProjectAvalonia.ViewModels;

namespace ProjectAvalonia.Features.Project.ViewModels;
public partial class ProjectExplorerViewModel : ViewModelBase
{
    [AutoNotify] private SolutionStateViewModel _solutionModel;
    [AutoNotify] private bool _isDocumentSolutionEnabled = false;

    [AutoNotify] private ItemModel _selectedItem;

    public ProjectExplorerViewModel()
    {

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
