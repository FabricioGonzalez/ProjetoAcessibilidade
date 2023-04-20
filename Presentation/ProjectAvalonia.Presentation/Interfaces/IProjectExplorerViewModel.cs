using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using ProjectAvalonia.Presentation.States;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IProjectExplorerViewModel : INotifyPropertyChanged
{
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
    }
}