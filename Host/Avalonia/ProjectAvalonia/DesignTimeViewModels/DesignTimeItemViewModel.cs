using System;
using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeItemViewModel
    : ReactiveObject
        , IItemViewModel
{
    public DesignTimeItemViewModel(
        IItemGroupViewModel parent
    )
    {
        Parent = parent;
    }

    public ReactiveCommand<IItemViewModel, Unit> RenameFileCommand
    {
        get;
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
        get; set;
    } = @"D:\PC-TI\Projetos\Desktop\ProjetoAcessibilidade\item.prjd";

    public string Name
    {
        get;
        set;
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

    public bool InEditing
    {
        get;
        set;
    }

    public ReactiveCommand<Unit, Unit> ExcludeFileCommand
    {
        get;
        set;
    }

    ReactiveCommand<Unit, Unit> IItemViewModel.RenameFileCommand
    {
        get;
    }

    public ReactiveCommand<IItemGroupViewModel, Unit> CanMoveCommand
    {
        get;
    }
}