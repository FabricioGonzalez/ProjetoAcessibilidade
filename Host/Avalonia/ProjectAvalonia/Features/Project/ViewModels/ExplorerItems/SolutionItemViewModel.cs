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
    private bool _isEditing;

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
                        target: NavigationTarget.CompactDialogScreen)).Result.Item2)
                {
                    /*await MoveItem(parent);*/
                }
            }, canExecute: Observable.Return(false));

        RenameFileCommand =
            ReactiveCommand.Create(execute: () => RenameFile(), canExecute: Observable.Return(false));
    }

    public bool InEditing
    {
        get => _isEditing;
        set => this.RaiseAndSetIfChanged(backingField: ref _isEditing, newValue: value);
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
        set;
    }

    public string Name
    {
        get;
        set;
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

    public ReactiveCommand<Unit, Unit> RenameFileCommand
    {
        get;
    }

    private void RenameFile() => InEditing = true;

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