using System.Collections.ObjectModel;
using System.Reactive;
using Common.Optional;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
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

    public ObservableCollection<IItemGroupViewModel> Items
    {
        get;
        set;
    } = new()
    {
        new DesignTimeItemGroupViewModel()
    };

    public ReactiveCommand<Unit, Optional<IItemGroupViewModel>> CreateFolderCommand
    {
        get;
    }

    SolutionState IProjectExplorerViewModel.SolutionState
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

    public void SetCurrentSolution(
        SolutionState state
    )
    {
    }
}