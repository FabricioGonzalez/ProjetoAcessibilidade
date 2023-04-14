using System;
using System.Collections.Generic;
using System.Reactive;
using Core.Entities.Solution.ItemsGroup;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeItemGroupViewModel : ReactiveObject, IItemGroupViewModel
{
    public DesignTimeItemGroupViewModel()
    {
        Items = new List<IItemViewModel>
        {
            new DesignTimeItemViewModel(parent: this),
            new DesignTimeItemViewModel(parent: this)
        };
    }

    public string Name
    {
        get;
    } = "solucao";

    public string ItemPath
    {
        get;
    } = @"D:\PC-TI\Projetos\Desktop\ProjetoAcessibilidade\item.prja";

    public IEnumerable<IItemViewModel> Items
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

    public void TransformFrom(List<ItemModel> items) => throw new NotImplementedException();
}