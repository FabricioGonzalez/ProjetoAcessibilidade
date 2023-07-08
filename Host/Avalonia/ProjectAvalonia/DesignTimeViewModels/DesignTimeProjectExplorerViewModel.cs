using System.Collections.ObjectModel;
using System.Reactive;
using Common.Optional;
using ProjectAvalonia.Presentation.Interfaces;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeProjectExplorerViewModel
    : ReactiveObject
        , IProjectExplorerViewModel
{
    public ReactiveCommand<IItemGroupViewModel, Unit> AddItemToProject
    {
        get;
    }

    public ReactiveCommand<Unit, Optional<IItemGroupViewModel>> CreateFolderCommand
    {
        get;
    }

    public ProjectSolutionModel SolutionState
    {
        get;
    }

    public ISolutionGroupViewModel SolutionRootItem
    {
        get;
        set;
    }

    public IItemViewModel SelectedItem
    {
        get;
        set;
    }

    public ObservableCollection<IItemGroupViewModel> Items
    {
        get;
        set;
    } = new()
    {
        new DesignTimeItemGroupViewModel()
    };

    public void SetCurrentSolution(
        ProjectSolutionModel state
    )
    {
    }
}