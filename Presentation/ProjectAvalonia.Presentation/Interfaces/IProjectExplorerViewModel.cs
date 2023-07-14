using System.ComponentModel;
using System.Reactive;
using Common.Optional;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IProjectExplorerViewModel : INotifyPropertyChanged
{
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

    public void SetCurrentSolution(
        ProjectSolutionModel state
    );
}