﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Common;
using Common.Optional;
using DynamicData.Binding;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.Features.Project.Services;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels.ExplorerItems;

public class ProjectExplorerViewModel
    : ViewModelBase
        , IProjectExplorerViewModel
{
    private readonly ItemsService _itemsService;
    private IItemViewModel _selectedItem;
    private ISolutionGroupViewModel _solutionGroupView;

    public ProjectExplorerViewModel(
        SolutionState state
        , ItemsService itemsService
    )
    {
        SolutionState = state;
        _itemsService = itemsService;

        SolutionRootItem = new SolutionGroupViewModel();

        SolutionRootItem.SolutionItem = new SolutionItemViewModel(id: "", itemPath: state.FilePath
            , name: Path.GetFileNameWithoutExtension(state.FilePath), templateName: "");

        SolutionRootItem.ConclusionItem = new ConclusionItemViewModel(id: Guid.NewGuid().ToString()
            , itemPath: Path.Combine(path1: Path.GetDirectoryName(state.FilePath), path2: "conclusion.prjc")
            , name: "Conclusão", templateName: "");

        SolutionRootItem.LocationItems = new ObservableCollection<ISolutionLocationItem>(SolutionState.LocationItems
            .Select(
                model =>
                {
                    var solutionLocation = new SolutionLocationItemViewModel(
                        name: model.Name,
                        itemPath: "",
                        saveSolution: async () => await SaveSolution(),
                        itemsService: _itemsService)
                    {
                        Items = new ObservableCollection<IItemGroupViewModel>(model.ItemGroup.Select(
                            vm =>
                            {
                                var item = new ItemGroupViewModel(
                                    name: model.Name,
                                    itemPath: "",
                                    itemsService: _itemsService,
                                    SaveSolution: async () => await SaveSolution());

                                item.MoveItemCommand = ReactiveCommand.CreateFromTask(SaveSolution);

                                item.TransformFrom(vm.Items.ToList());

                                return item;
                            }
                        ))
                    };

                    solutionLocation.MoveItemCommand = ReactiveCommand.CreateFromTask(SaveSolution);

                    return solutionLocation;
                }));

        this.WhenAnyValue(vm => vm.SelectedItem)
            .WhereNotNull()
            .Subscribe(it =>
            {
                Debug.WriteLine(it.Name);
            });

        var changeSet = SolutionRootItem.LocationItems
            .ToObservableChangeSet();

        /*changeSet.AutoRefreshOnObservable(document => document.AddProjectItemCommand.IsExecuting)
            .Select(x => WhenAnyItemWasAdded())
            .Switch()
            .SubscribeAsync(async item =>
            {
                await SaveSolution();
            });

        changeSet
            .AutoRefreshOnObservable(document => document.ExcludeFolderCommand.IsExecuting)
            .Select(x => WhenAnyFolderIsDeleted())
            .Switch()
            .SubscribeAsync(async x =>
            {
                var dialog = new DeleteDialogViewModel(
                    "O item seguinte será excluido ao confirmar. Deseja continuar?", "Deletar Item"
                    , "");

                if ((await RoutableViewModel.NavigateDialogAsync(dialog,
                        NavigationTarget.CompactDialogScreen)).Result)
                {
                    _ = SolutionRootItem.LocationItems.Remove(x);

                    _ = await _mediator.Send(new DeleteProjectFolderItemCommand(x.ItemPath), CancellationToken.None);

                    SolutionState.RemoveFromSolution(i => i.Name == x.Name);

                    await SaveSolution();
                }
            });*/

        CreateFolderCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var dialog = new CreateFolderViewModel(
                message: "Defina o nome da pasta", title: "Criar Pasta"
                , caption: "",
                items: SolutionRootItem.LocationItems);

            var result = await RoutableViewModel.NavigateDialogAsync(dialog: dialog,
                target: NavigationTarget.CompactDialogScreen);

            if (result.Kind == DialogResultKind.Normal)
            {
                var item = new SolutionLocationItemViewModel(
                    name: result.Result,
                    itemPath: Path.Combine(path1: Directory.GetParent(SolutionState.FilePath).FullName
                        , path2: Constants.AppProjectItemsFolderName, path3: result.Result),
                    saveSolution: async () => await SaveSolution(),
                    itemsService: _itemsService);

                SolutionRootItem.LocationItems.Add(item);

                _itemsService.SyncSolutionItems(SolutionRootItem);

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
                    , CancellationToken.None);
                return Optional<IItemGroupViewModel>.Some(item);*/
            }

            return Optional<IItemGroupViewModel>.None();
        });

        CreateFolderCommand
            .DoAsync(async _ => await SaveSolution())
            .Subscribe();
    }

    public ReactiveCommand<IItemViewModel, Unit> SetEditingItem
    {
        get;
        set;
    } = ReactiveCommand.Create<IItemViewModel>(it =>
    {
    });

    public ISolutionGroupViewModel SolutionRootItem
    {
        get => _solutionGroupView;
        set => this.RaiseAndSetIfChanged(backingField: ref _solutionGroupView, newValue: value);
    }

    public IItemViewModel SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(backingField: ref _selectedItem, newValue: value);
    }

    public void SetCurrentSolution(
        SolutionState state
    ) => SolutionState = state;

    public ReactiveCommand<Unit, Optional<IItemGroupViewModel>> CreateFolderCommand
    {
        get;
    }

    public SolutionState SolutionState
    {
        get;
        private set;
    }

    /*private IObservable<IItemViewModel?> WhenAnyItemWasAdded() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        SolutionRootItem.LocationItems
            .Select(x => x.AddProjectItemCommand.Select(s => s))
            .Merge();

    private IObservable<IItemGroupViewModel?> WhenAnyItemWasMoved() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        SolutionRootItem.LocationItems
            .Select(x => x.MoveItemCommand.Select(_ => x))
            .Merge();*/


    private async Task SaveSolution() => _itemsService.SyncSolutionItems(SolutionRootItem);
    /*SolutionState?.ReloadItem(SolutionRootItem.LocationItems
            .Select(solutionItem => new SolutionGroupModel
            {
                Name = solutionItem.Name, ItemPath = solutionItem.ItemPath, Items = solutionItem.Items
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
            }).ToList());

        _ = await _mediator.Send(
            new CreateSolutionCommand(
                SolutionState?.FilePath,
                SolutionState),
            CancellationToken.None);*/
    /*private IObservable<IItemGroupViewModel?> WhenAnyFolderIsDeleted() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        SolutionRootItem.LocationItems
            .Select(x => x.ExcludeFolderCommand.Select(_ => x))
            .Merge();*/
}