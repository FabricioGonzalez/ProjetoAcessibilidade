using System;
using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeItemViewModel : ReactiveObject, IItemViewModel
{
    public DesignTimeItemViewModel(IItemGroupViewModel parent)
    {
        Parent = parent;
    }

    public IItemGroupViewModel Parent
    {
        get;
    }

    public string Id
    {
        get;
    } = Guid.NewGuid().ToString();

    public string ItemPath
    {
        get;
    } = @"D:\PC-TI\Projetos\Desktop\ProjetoAcessibilidade\item.prjd";

    public string Name
    {
        get;
    } = "Teste";

    public string TemplateName
    {
        get;
    } = "Teste";

    public ReactiveCommand<IItemGroupViewModel, Unit> CommitFileCommand
    {
        get;
    }

    public ReactiveCommand<IItemViewModel, Unit> SelectItemToEditCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> ExcludeFileCommand
    {
        get;
        set;
    }

    public ReactiveCommand<IItemViewModel, Unit> RenameFileCommand
    {
        get;
    }

    public ReactiveCommand<IItemGroupViewModel, Unit> CanMoveCommand
    {
        get;
    }
}