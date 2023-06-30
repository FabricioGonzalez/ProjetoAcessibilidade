using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common;

using DynamicData;
using DynamicData.Binding;

using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;

using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ItemGroupViewModel : ReactiveObject,
    IItemGroupViewModel
{
    private readonly IMediator _mediator;

    public ItemGroupViewModel(
        string name,
        string itemPath,
        Func<Task> SaveSolution
    )
    {
        _mediator = Locator.Current.GetService<IMediator>();

        Name = name;
        ItemPath = itemPath;

        Items = new ObservableCollection<IItemViewModel>();
        CommitFolderCommand = ReactiveCommand.CreateFromTask(execute: CommitFolder);
        AddProjectItemCommand = ReactiveCommand.CreateFromTask(execute: AddProjectItem);
        RenameFolderCommand = ReactiveCommand.CreateFromTask(execute: renameItem);

        // Whenever the list of documents change, calculate a new Observable
        // to represent whenever any of the *current* documents have been
        // requested to close, then Switch to that. When we get something
        // to close, remove it from the list.

        _ = Items
            .ToObservableChangeSet()
            .AutoRefreshOnObservable(reevaluator: document => document.ExcludeFileCommand.IsExecuting)
            .Select(selector: x => WhenAnyDocumentClosed())
            .Switch()
            .SubscribeAsync(
                onNextAsync: async x =>
                {
                    var dialog = new DeleteDialogViewModel(
                        message: "O item seguinte será excluido ao confirmar. Deseja continuar?",
                        title: "Deletar Item",
                        caption: "");

                    if ((await RoutableViewModel.NavigateDialogAsync(
                            dialog: dialog,
                            target: NavigationTarget.CompactDialogScreen)).Result)
                    {
                        _ = Items.Remove(item: x);

                        _ = await _mediator.Send(new DeleteProjectFileItemCommand(x.ItemPath), CancellationToken.None);
                        await SaveSolution();
                    }
                });

        _ = Items
            .ToObservableChangeSet()
            .AutoRefreshOnObservable(reevaluator: document => document.SelectItemToEditCommand.IsExecuting)
            .Select(selector: x => WhenAnyItemIsSelected())
            .Switch()
            .SubscribeAsync(
                onNextAsync: async x =>
                {
                    var result = ProjectEditingViewModel
                    .SetEditingItem
                    .Handle(input: x)
                        .Subscribe();
                });
    }

    public ReactiveCommand<Unit, Unit> SelectItemToEdit
    {
        get;
    }

    public ObservableCollection<IItemViewModel> Items
    {
        get;
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
        get; set;
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
        execute: () =>
        {
        });

    public void TransformFrom(
        List<ItemModel> items
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

            Items.Add(item: itemToAdd);
        }
    }

    private IObservable<IItemViewModel?> WhenAnyItemIsSelected() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        Items
            .Select(selector: x => x.SelectItemToEditCommand.Select(selector: _ => x))
            .Merge();

    private async Task renameItem()
    {
    }

    private IObservable<IItemViewModel?> WhenAnyDocumentClosed() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        Items
            .Select(selector: x => x.ExcludeFileCommand.Select(selector: _ => x))
            .Merge();

    private async Task<IItemViewModel?> AddProjectItem()
    {
        var addItemViewModel = new AddItemViewModel(Items);

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
                item: item);

            _ = (await _mediator.Send(
                request: new CreateItemCommand(
                    ItemPath: path,
                    ItemName: dialogResult.Result.TemplateName),
                cancellation: CancellationToken.None))
                .IfFail(error =>
                {
                    NotificationHelpers.Show("Erro ao criar item", error.Message);
                });

            return item;
        }
        return null;
    }

    private async Task CommitFolder()
    {
    }
}