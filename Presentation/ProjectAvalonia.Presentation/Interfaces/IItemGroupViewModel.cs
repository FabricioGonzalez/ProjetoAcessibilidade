using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;

using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;

using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IItemGroupViewModel : INotifyPropertyChanged
{
    public string Name
    {
        get;
    }

    public string ItemPath
    {
        get;
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


    /*public void RemoveItem(IItemViewModel item);*/

    public void TransformFrom(List<ItemModel> items);
}