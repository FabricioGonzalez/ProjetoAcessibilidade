using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;

using ProjectAvalonia.Presentation.States.ProjectItems;

using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IItemGroupViewModel : INotifyPropertyChanged
{
    public string Name
    {
        get;
        set;
    }

    public string ItemPath
    {
        get;
        set;
    }
    public ISolutionLocationItem Parent
    {
        get; set;
    }
    public ObservableCollection<IItemViewModel> Items
    {
        get;
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

    public ReactiveCommand<Unit, IItemViewModel> AddProjectItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> ExcludeFolderCommand
    {
        get;
    }

    public bool InEditing
    {
        get;
        set;
    }

    /*public void RemoveItem(IItemViewModel item);*/

    public void TransformFrom(
        List<ItemState> items
    );
}