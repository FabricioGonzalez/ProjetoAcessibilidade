using System.ComponentModel;
using System.Reactive;
using Core.Entities.Solution.ItemsGroup;
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

    IEnumerable<IItemViewModel> Items
    {
        get;
    }

    public ReactiveCommand<IItemGroupViewModel, Unit> CommitFolderCommand
    {
        get;
    }

    public ReactiveCommand<IItemGroupViewModel, Unit> AddProjectItemCommand
    {
        get;
    }

    public ReactiveCommand<IItemGroupViewModel, Unit> ExcludeFolderCommand
    {
        get;
    }

    public void TransformFrom(List<ItemModel> items);
}