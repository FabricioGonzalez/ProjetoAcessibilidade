using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData.Binding;
using ProjectAvalonia.Features.Project.Services;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class SolutionLocationItemViewModel
    : ReactiveObject
        , ISolutionLocationItem
{
    private readonly ItemsService _itemsService;

    private readonly Func<Task> SaveSolution;

    public SolutionLocationItemViewModel(
        string name
        , string itemPath
        , Func<Task> saveSolution
        , ItemsService itemsService
    )
    {
        SaveSolution = saveSolution;
        _itemsService = itemsService;

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
            message: "Defina o nome da pasta", title: "Criar Pasta"
            , caption: "",
            items: Items);

        var result = await RoutableViewModel.NavigateDialogAsync(dialog: dialog,
            target: NavigationTarget.CompactDialogScreen);

        if (result.Kind == DialogResultKind.Normal)
        {
            var item = new ItemGroupViewModel(
                name: result.Result,
                itemPath: Path.Combine(path1: ItemPath
                    , path2: result.Result),
                itemsService: _itemsService,
                SaveSolution: SaveSolution
            );

            Items.Add(item);

            await SaveSolution();

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
            /*_ = await _mediator.Send(new CreateSolutionItemFolderCommand(item.Name, item.ItemPath)
                , CancellationToken.None);*/

            return item;
            /*return Optional<IItemGroupViewModel>.Some(item);*/
        }

        return null;
    }


    private async Task CommitFolder()
    {
    }
}