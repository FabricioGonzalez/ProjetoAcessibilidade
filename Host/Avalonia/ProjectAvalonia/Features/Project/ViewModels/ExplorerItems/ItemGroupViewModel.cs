using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Common;
using DynamicData;
using DynamicData.Binding;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Features.Project.Services;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.ProjectItems;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ItemGroupViewModel
    : ReactiveObject
        , IItemGroupViewModel
{
    private readonly ItemsService _itemsService;
    private readonly Func<Task> _saveSolution;

    public ItemGroupViewModel(
        string name
        , string itemPath
        , ItemsService itemsService
        , Func<Task> SaveSolution
    )
    {
        _itemsService = itemsService;
        _saveSolution = SaveSolution;
        Name = name;
        ItemPath = itemPath;

        Items = new ObservableCollection<IItemViewModel>();
        CommitFolderCommand = ReactiveCommand.CreateFromTask(CommitFolder);
        AddProjectItemCommand = ReactiveCommand.CreateFromTask(AddProjectItem);
        RenameFolderCommand = ReactiveCommand.CreateFromTask(renameItem);

        // Whenever the list of documents change, calculate a new Observable
        // to represent whenever any of the *current* documents have been
        // requested to close, then Switch to that. When we get something
        // to close, remove it from the list.

        var itemsObservable = Items
            .ToObservableChangeSet();

        itemsObservable
            .AutoRefreshOnObservable(document => document.ExcludeFileCommand.IsExecuting)
            .Select(x => WhenAnyDocumentClosed())
            .Switch()
            .SubscribeAsync(
                async x =>
                {
                    var dialog = new DeleteDialogViewModel(
                        message: "O item seguinte será excluido ao confirmar. Deseja continuar?",
                        title: "Deletar Item",
                        caption: "");

                    if ((await RoutableViewModel.NavigateDialogAsync(
                            dialog: dialog,
                            target: NavigationTarget.CompactDialogScreen)).Result)
                    {
                        _ = Items.Remove(x);

                        /*_ = await _mediator.Send(new DeleteProjectFileItemCommand(x.ItemPath), CancellationToken.None);*/
                        await SaveSolution();
                    }
                });

        itemsObservable
            .AutoRefreshOnObservable(document => document.SelectItemToEditCommand.IsExecuting)
            .Select(x => WhenAnyItemIsSelected())
            .Switch()
            .InvokeCommand(SelectItemToEdit);
    }

    public ReactiveCommand<IItemViewModel, Unit> SelectItemToEdit
    {
        get;
        set;
    } = ReactiveCommand.Create<IItemViewModel>(it =>
    {
    });

    public ObservableCollection<IItemViewModel> Items
    {
        get;
        init;
    }


    public string Name
    {
        get;
    }

    public string ItemPath
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> CommitFolderCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> RenameFolderCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> MoveItemCommand
    {
        get;
        set;
    }
        = ReactiveCommand.CreateFromObservable(() => Observable.Return(Unit.Default));

    public ReactiveCommand<Unit, IItemViewModel> AddProjectItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> ExcludeFolderCommand
    {
        get;
    } = ReactiveCommand.Create(
        () =>
        {
        });

    public void TransformFrom(
        List<ItemState> items
    )
    {
        foreach (var item in items)
        {
            var itemToAdd = new ItemViewModel(
                id: item.Id,
                itemPath: item.ItemPath,
                name: item.Name,
                templateName: item.TemplateName,
                parent: this);

            Items.Add(itemToAdd);
        }
    }

    private IObservable<IItemViewModel?> WhenAnyItemIsSelected() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        Items
            .Select(x => x.SelectItemToEditCommand.Select(_ => x))
            .Merge();

    private async Task renameItem()
    {
    }

    private IObservable<IItemViewModel?> WhenAnyDocumentClosed() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        Items
            .Select(x => x.ExcludeFileCommand.Select(_ => x))
            .Merge();

    private async Task<IItemViewModel?> AddProjectItem()
    {
        var addItemViewModel = new AddItemViewModel(fileItems: Items, itemsService: _itemsService);

        var dialogResult = await RoutableViewModel.NavigateDialogAsync(
            dialog: addItemViewModel,
            target: NavigationTarget.DialogScreen);

        if (dialogResult.Kind is DialogResultKind.Normal &&
            dialogResult.Result is not null)
        {
            var path = Path.Combine(
                path1: ItemPath,
                path2: $"{dialogResult.Result.Name}{Constants.AppProjectItemExtension}");

            var item = new ItemViewModel(
                id: Guid.NewGuid()
                    .ToString(),
                itemPath: path,
                name: dialogResult.Result.Name,
                templateName: dialogResult.Result.TemplateName,
                parent: this);

            Items.Add(
                item);
            await _saveSolution();
            /*
            _ = (await _mediator.Send(
                    new CreateItemCommand(
                        path,
                        dialogResult.Result.TemplateName),
                    CancellationToken.None))
                .IfFail(error =>
                {
                    NotificationHelpers.Show("Erro ao criar item", error.Message);
                });
                */

            return item;
        }

        return null;
    }

    private async Task CommitFolder()
    {
    }
}