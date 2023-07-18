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

public class ItemGroupViewModel
    : ReactiveObject
        , IItemGroupViewModel
{
    private readonly IMediator _mediator;

    public ItemGroupViewModel(
        string name
        , string itemPath
        , Func<Task> SaveSolution
    )
    {
        _mediator = Locator.Current.GetService<IMediator>();

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
                        "O item seguinte será excluido ao confirmar. Deseja continuar?",
                        "Deletar Item",
                        "");

                    if ((await RoutableViewModel.NavigateDialogAsync(
                            dialog,
                            NavigationTarget.CompactDialogScreen)).Result)
                    {
                        _ = Items.Remove(x);

                        _ = await _mediator.Send(new DeleteProjectFileItemCommand(x.ItemPath), CancellationToken.None);
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
        List<ItemModel> items
    )
    {
        foreach (var item in items)
        {
            var itemToAdd = new ItemViewModel(
                item.Id,
                item.ItemPath,
                item.Name,
                item.TemplateName,
                this);

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
        var addItemViewModel = new AddItemViewModel(Items);

        var dialogResult = await RoutableViewModel.NavigateDialogAsync(
            addItemViewModel,
            NavigationTarget.DialogScreen);

        if (dialogResult.Kind is DialogResultKind.Normal &&
            dialogResult.Result is not null)
        {
            var path = Path.Combine(
                ItemPath,
                $"{dialogResult.Result.Name}{Constants.AppProjectItemExtension}");

            var item = new ItemViewModel(
                Guid.NewGuid()
                    .ToString(),
                path,
                dialogResult.Result.Name,
                dialogResult.Result.TemplateName,
                this);

            Items.Add(
                item);

            _ = (await _mediator.Send(
                    new CreateItemCommand(
                        path,
                        dialogResult.Result.TemplateName),
                    CancellationToken.None))
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