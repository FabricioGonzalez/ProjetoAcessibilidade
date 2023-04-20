using System.Collections.ObjectModel;
using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeProjectExplorerViewModel : ReactiveObject, IProjectExplorerViewModel
{
    public ReactiveCommand<IItemGroupViewModel, Unit> AddItemToProject
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> CreateFolderCommand
    {
        get;
    }

    public SolutionState SolutionState
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
}