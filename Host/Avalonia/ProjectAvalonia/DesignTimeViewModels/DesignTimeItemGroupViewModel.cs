using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.ProjectItems;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeItemGroupViewModel
    : ReactiveObject
        , IItemGroupViewModel
{
    public DesignTimeItemGroupViewModel()
    {
        Items = new ObservableCollection<IItemViewModel>
        {
            new DesignTimeItemViewModel(parent: this), new DesignTimeItemViewModel(parent: this)
        };
    }

    public ObservableCollection<IItemViewModel> Items
    {
        get;
    }

    public string Name
    {
        get;
        set;
    } = "solucao";

    public string ItemPath
    {
        get; set;
    } = @"D:\PC-TI\Projetos\Desktop\ProjetoAcessibilidade\item.prja";

    public ReactiveCommand<Unit, Unit> CommitFolderCommand
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

    public void TransformFrom(
        List<ItemState> items
    )
    {
    }

    public ReactiveCommand<Unit, Unit> MoveItemCommand
    {
        get;
    }

    public void RemoveItem(
        IItemViewModel item
    )
    {
    }
}