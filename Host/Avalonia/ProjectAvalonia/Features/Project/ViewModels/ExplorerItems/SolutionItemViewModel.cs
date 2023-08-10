using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class SolutionItemViewModel
    : ReactiveObject
        , IItemViewModel
{
    /*private readonly IMediator _mediator;*/

    public SolutionItemViewModel(
        string id
        , string itemPath
        , string name
        , string templateName
    )
    {
        ItemPath = itemPath;
        Id = id;
        Name = name;
        TemplateName = templateName;

        /*_mediator = Locator.Current.GetService<IMediator>();*/

        CommitFileCommand =
            ReactiveCommand.CreateFromTask<IItemGroupViewModel>(execute: CommitFile
                , canExecute: Observable.Return(false));
        SelectItemToEditCommand = ReactiveCommand.CreateFromTask<IItemViewModel>(SelectItemToEdit);
        ExcludeFileCommand =
            ReactiveCommand.Create(execute: () =>
            {
            }, canExecute: Observable.Return(false));
        CanMoveCommand =
            ReactiveCommand.CreateFromTask<IItemGroupViewModel>(execute: async parent =>
            {
                var dialog = new DeleteDialogViewModel(
                    message: "O item seguinte será excluido ao confirmar. Deseja continuar?",
                    title: "Deletar Item",
                    caption: "");

                if ((await RoutableViewModel.NavigateDialogAsync(
                        dialog: dialog,
                        target: NavigationTarget.CompactDialogScreen)).Result)
                {
                    /*await MoveItem(parent);*/
                }
            }, canExecute: Observable.Return(false));

        RenameFileCommand =
            ReactiveCommand.CreateFromTask<IItemViewModel>(execute: RenameFile, canExecute: Observable.Return(false));
    }

    public string Id
    {
        get;
    }

    public IItemGroupViewModel Parent
    {
        get;
    }

    public string ItemPath
    {
        get;
    }

    public string Name
    {
        get;
    }

    public string TemplateName
    {
        get;
    }

    public ReactiveCommand<IItemGroupViewModel, Unit> CommitFileCommand
    {
        get;
    }

    public ReactiveCommand<IItemGroupViewModel, Unit> CanMoveCommand
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

    private Task RenameFile(
        IItemViewModel item
        , CancellationToken token
    ) => Task.CompletedTask;

    private async Task SelectItemToEdit(
        IItemViewModel item
        , CancellationToken token
    ) =>
        ProjectEditingViewModel
            .SetEditingItem
            .Handle(item)
            .Subscribe();

    private Task CommitFile(
        IItemGroupViewModel item
        , CancellationToken token
    ) => Task.CompletedTask;
}