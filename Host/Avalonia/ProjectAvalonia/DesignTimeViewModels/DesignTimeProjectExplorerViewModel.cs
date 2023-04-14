using System.Collections.Generic;
using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeProjectExplorerViewModel : ReactiveObject, IProjectExplorerViewModel
{
    public ReactiveCommand<Unit, Unit> CreateFolderCommand
    {
        get;
    }

    public SolutionState SolutionState
    {
        get;
    }

    public List<IItemGroupViewModel> Items
    {
        get;
        set;
    } = new()
    {
        new DesignTimeItemGroupViewModel()
    };
}