using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IProjectExplorerViewModel : INotifyPropertyChanged
{
    public ReactiveCommand<Unit, Unit> CreateFolderCommand
    {
        get;
    }


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