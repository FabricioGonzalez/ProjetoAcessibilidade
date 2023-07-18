﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using DynamicData.Binding;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Solution.Commands.SolutionItem;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class SolutionLocationItemViewModel
    : ReactiveObject
        , ISolutionLocationItem
{
    private readonly IMediator _mediator;
    private readonly Func<Task> SaveSolution;

    public SolutionLocationItemViewModel(
        string name
        , string itemPath
        , Func<Task> saveSolution
    )
    {
        SaveSolution = saveSolution;
        _mediator = Locator.Current.GetService<IMediator>();

        Name = name;
        ItemPath = itemPath;

        Items = new ObservableCollection<IItemGroupViewModel>();
        CommitFolderCommand = ReactiveCommand.CreateFromTask(CommitFolder);
        AddProjectItemCommand = ReactiveCommand.CreateFromTask(AddProjectItem);
        RenameFolderCommand = ReactiveCommand.CreateFromTask(renameItem);

        // Whenever the list of documents change, calculate a new Observable
        // to represent whenever any of the *current* documents have been
        // requested to close, then Switch to that. When we get something
        // to close, remove it from the list.

        var itemsObservable = Items
            .ToObservableChangeSet();

        /*itemsObservable
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
            .InvokeCommand(SelectItemToEdit);*/
    }

    public ReactiveCommand<IItemViewModel, Unit> SelectItemToEdit
    {
        get;
        set;
    } = ReactiveCommand.Create<IItemViewModel>(it =>
    {
    });

    public ObservableCollection<IItemGroupViewModel> Items
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

    public ReactiveCommand<Unit, IItemGroupViewModel> AddProjectItemCommand
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

    /*private IObservable<IItemViewModel?> WhenAnyItemIsSelected() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        Items
            .Select(x => x.SelectItemToEditCommand.Select(_ => x))
            .Merge();*/

    private async Task renameItem()
    {
    }

    /*private IObservable<IItemViewModel?> WhenAnyDocumentClosed() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        Items
            .Select(x => x.ExcludeFileCommand.Select(_ => x))
            .Merge();*/

    private async Task<IItemGroupViewModel?> AddProjectItem()
    {
        var dialog = new CreateFolderViewModel(
            "Defina o nome da pasta", "Criar Pasta"
            , "",
            Items);

        var result = await RoutableViewModel.NavigateDialogAsync(dialog,
            NavigationTarget.CompactDialogScreen);

        if (result.Kind == DialogResultKind.Normal)
        {
            var item = new ItemGroupViewModel(
                result.Result,
                Path.Combine(ItemPath
                    , Constants.AppProjectItemsFolderName, result.Result),
                SaveSolution
            );

            Items.Add(item);

            /*SolutionState.AddItemToSolution(new SolutionGroupModel
            {
                Name = result.Result, ItemPath = item.ItemPath, Items = item.Items
                    .Select(it => new ItemGroupModel
                    {
                        Name = it.Name, ItemPath =
                            it.ItemPath
                        , Items = it.Items
                            .Select(x => new ItemModel
                            {
                                Id = x.Id, ItemPath = x.ItemPath, Name = x.Name, TemplateName = x.TemplateName
                            })
                            .ToList()
                    }).ToList()
            });*/
            _ = await _mediator.Send(new CreateSolutionItemFolderCommand(item.Name, item.ItemPath)
                , CancellationToken.None);

            return item;
            /*return Optional<IItemGroupViewModel>.Some(item);*/
        }

        return null;
    }


    private async Task CommitFolder()
    {
    }
}