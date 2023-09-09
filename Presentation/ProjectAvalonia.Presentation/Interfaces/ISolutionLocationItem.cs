using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ISolutionLocationItem : INotifyPropertyChanged
{
    public string Name
    {
        get;
    }

    public string ItemPath
    {
        get;
    }

    public ObservableCollection<IItemGroupViewModel> Items
    {
        get;
    }

    public bool InEditing
    {
        get;
        set;
    }

    public ReactiveCommand<Unit, Unit> CommitFolderCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> MoveItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> RenameFolderCommand
    {
        get;
    }

    public ReactiveCommand<Unit, IItemGroupViewModel> AddProjectItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> ExcludeFolderCommand
    {
        get;
    }
}