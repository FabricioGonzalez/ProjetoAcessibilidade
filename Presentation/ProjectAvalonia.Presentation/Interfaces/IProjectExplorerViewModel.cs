using System.Collections.ObjectModel;
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

    public void SetCurrentSolution(ProjectSolutionModel state);
    public ProjectSolutionModel SolutionState
    {
        get;
    }

    public ObservableCollection<IItemGroupViewModel> Items
    {
        get;
        set;
    }
}