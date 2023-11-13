using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Common.Linq;

using DynamicData;
using DynamicData.Binding;

using ProjectAvalonia.Common.Extensions;
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
    private readonly EditingItemsNavigationService _editableItemsNavigationService;
    private readonly Func<Task> SaveSolution;

    /*private IObservable<IItemViewModel?> WhenAnyItemIsSelected() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        Items
            .Select(x => x.SelectItemToEditCommand.Select(_ => x))
            .Merge();*/
    private bool _isEditing;

    public SolutionLocationItemViewModel(
        string name
        , string itemPath
        , Func<Task> saveSolution
        , ItemsService itemsService,
        EditingItemsNavigationService editableItemsNavigationService
    )
    {
        SaveSolution = saveSolution;
        _itemsService = itemsService;
        _editableItemsNavigationService = editableItemsNavigationService;
        Name = name;
        ItemPath = itemPath;

        Items = new ObservableCollection<IItemGroupViewModel>();
        CommitFolderCommand = ReactiveCommand.CreateFromTask(CommitFolder);
        AddProjectItemCommand = ReactiveCommand.CreateFromTask(AddProjectItem);
        RenameFolderCommand = ReactiveCommand.Create(RenameFile);

        // Whenever the list of documents change, calculate a new Observable
        // to represent whenever any of the *current* documents have been
        // requested to close, then Switch to that. When we get something
        // to close, remove it from the list.

        var itemsObservable = Items
            .ToObservableChangeSet();

        itemsObservable
            .AutoRefreshOnObservable(document => document.ExcludeFolderCommand.IsExecuting)
            .Select(x => WhenAnyDocumentClosed())
            .Switch()
            .SubscribeAsync(
                async x =>
                {
                    var dialog = new DeleteDialogViewModel(
                        message: "O item seguinte será excluido ao confirmar. Deseja continuar?",
                        title: "Deletar Item",
                        caption: "");
                    var result = (await RoutableViewModel.NavigateDialogAsync(
                        dialog: dialog,
                        target: NavigationTarget.CompactDialogScreen)).Result;
                    if (result.Item2)
                    {
                        if (result.Item1)
                        {
                            _itemsService.ExcludeFolder(x.ItemPath);
                        }

                        await SaveSolution();
                    }
                });
    }

    public ReactiveCommand<IItemViewModel, Unit> SelectItemToEdit
    {
        get;
        set;
    } = ReactiveCommand.Create<IItemViewModel>(it =>
    {
    });

    public ReactiveCommand<Unit, IItemGroupViewModel> AddExistingItemCommand
    {
        get;
    }

    public bool InEditing
    {
        get => _isEditing;
        set => this.RaiseAndSetIfChanged(backingField: ref _isEditing, newValue: value);
    }

    public ObservableCollection<IItemGroupViewModel> Items
    {
        get;
        set;
    }

    public string Name
    {
        get; set;
    }

    public string ItemPath
    {
        get;
        private set;
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
        init;
    } = ReactiveCommand.Create(
        () =>
        {
        });

    private void RenameFile() => InEditing = true;

    private IObservable<IItemGroupViewModel?> WhenAnyDocumentClosed() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        Items
            .Select(x => x.ExcludeFolderCommand.Select(_ => x))
            .Merge();

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
                SaveSolution: SaveSolution,
                this,
                _editableItemsNavigationService
            );

            Items.Add(item);

            await SaveSolution();

            return item;
        }

        return null;
    }


    private async Task CommitFolder()
    {
        if (ItemPath is { Length: > 0 } path)
        {
            var result = path.Split(Path.DirectorySeparatorChar)[..^1];

            var newPath = string.Join(separator: Path.DirectorySeparatorChar
                , values: result.Append(Name));

            if (Items.Any())
            {
                Items.IterateOn(it =>
                {
                    var childPath = it.ItemPath.Split(Path.DirectorySeparatorChar)[^1];

                    it.ItemPath = Path.Combine(path1: newPath, path2: childPath);

                    _editableItemsNavigationService.AlterItem((item) =>
                    {
                        item.ItemPath = it.ItemPath
                        .SplitPath(Path.DirectorySeparatorChar)
                        .Append(Path.GetFileName(item.ItemPath))
                        .JoinPath(Path.DirectorySeparatorChar);

                        item.ItemName = item.ItemPath.GetFileNameWithoutExtension();

                        item.DisplayName = it.ItemPath
                        .SplitPath(new Range(^2, ^0), Path.DirectorySeparatorChar)
                        .Append(item.ItemName)
                        .JoinPath(Path.DirectorySeparatorChar);

                        return item;
                    }, it2 =>
                    {

                        var itemPath = it2
                        .ItemPath
                        .SplitPath(new Range(^2, ^0), Path.DirectorySeparatorChar)
                        .Prepend(path);

                        var joinedPath = itemPath.JoinPath(Path.DirectorySeparatorChar);

                        return it2.ItemPath == joinedPath;
                    });
                });
            }

            _itemsService.RenameFolder(oldPath: path, newPath: newPath);

            ItemPath = newPath;

            await SaveSolution();
        }
    }
}