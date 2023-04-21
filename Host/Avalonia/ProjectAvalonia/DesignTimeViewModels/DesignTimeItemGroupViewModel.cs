using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Core.Entities.Solution.ItemsGroup;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeItemGroupViewModel : ReactiveObject, IItemGroupViewModel
{
    public DesignTimeItemGroupViewModel()
    {
        Items = new ObservableCollection<IItemViewModel>
        {
            new DesignTimeItemViewModel(parent: this),
            new DesignTimeItemViewModel(parent: this)
        };
    }

    public ObservableCollection<IItemViewModel> Items
    {
        get;
    }

    public string Name
    {
        get;
    } = "solucao";

    public string ItemPath
    {
        get;
    } = @"D:\PC-TI\Projetos\Desktop\ProjetoAcessibilidade\item.prja";

    public ReactiveCommand<Unit, Unit> CommitFolderCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> RenameFolderCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> AddProjectItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> ExcludeFolderCommand
    {
        get;
    }

    public void TransformFrom(List<ItemModel> items)
    {
    }

    public void RemoveItem(IItemViewModel item)
    {
    }
}