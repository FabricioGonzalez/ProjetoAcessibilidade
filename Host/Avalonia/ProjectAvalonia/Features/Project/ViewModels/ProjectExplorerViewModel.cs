using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Input;
using Common;
using Core.Entities.Solution.Explorer;
using DynamicData.Binding;
using Project.Domain.App.Models;
using Project.Domain.Contracts;
using Project.Domain.Project.Commands.ProjectItems;
using Project.Domain.Solution.Commands.SolutionItem;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Logging;
using ProjectAvalonia.Stores;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class ProjectExplorerViewModel : ViewModelBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly EditingItemsStore _editingItemsStore;
    private readonly ExplorerItemsStore _explorerItemsStore;
    private readonly SolutionStore _solutionStore;
    [AutoNotify] private bool _isDocumentSolutionEnabled;
    [AutoNotify] private ReadOnlyObservableCollection<ItemGroupState> _items;
    [AutoNotify] private string _phoneMask = "(00)0000-0000";

    [AutoNotify] private ItemState _selectedItem;

    public ProjectExplorerViewModel()
    {
        _solutionStore ??= Locator.Current.GetService<SolutionStore>();
        _editingItemsStore ??= Locator.Current.GetService<EditingItemsStore>();
        _explorerItemsStore ??= Locator.Current.GetService<ExplorerItemsStore>();
        _commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();


        this.WhenValueChanged(propertyAccessor: vm =>
                vm
                    .CurrentOpenSolution
                    .ReportData
                    .Telefone).Where(predicate: value => !string.IsNullOrWhiteSpace(value: value))
            .Subscribe(onNext: value =>
            {
                var val = value
                    .Replace(oldValue: "(", newValue: "")
                    .Replace(oldValue: ")", newValue: "")
                    .Replace(oldValue: "-", newValue: "")
                    .Replace(oldValue: "_", newValue: "");

                if (val
                        .Length <= 10)
                {
                    PhoneMask = "(00)0000-0000";
                }
                else
                {
                    PhoneMask = "(00)00000-0000";
                }

                Logger.LogDebug(message: $"{PhoneMask} - {val}");
            });

        this.WhenAnyValue(property1: vm => vm._solutionStore.CurrentOpenSolution.ItemGroups)
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Subscribe(onNext: prop =>
            {
                Items = prop;
            });

        RenameFileCommand = ReactiveCommand.Create<ItemState>(execute: model =>
        {
            Logger.LogInfo(message: model.Name);
        });

        ExcludeFolderCommand = ReactiveCommand.CreateFromTask<ItemGroupState>(execute: async groupModels =>
        {
            var dialog = new DeleteDialogViewModel(
                message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item"
                , caption: "");

            if ((await RoutableViewModel.NavigateDialogAsync(dialog: dialog,
                    target: NavigationTarget.CompactDialogScreen)).Result)
            {
                _solutionStore.CurrentOpenSolution.DeleteFolderItem(item: groupModels);
            }
        });

        AddProjectItemCommand = ReactiveCommand.CreateFromTask<ItemGroupState>(execute: async groupModels =>
        {
            var addItemViewModel = new AddItemViewModel();

            var dialogResult = await RoutableViewModel.NavigateDialogAsync(dialog: addItemViewModel,
                target: NavigationTarget.DialogScreen);
            if (dialogResult.Kind is DialogResultKind.Normal && dialogResult.Result is not null)
            {
                _solutionStore.CurrentOpenSolution.AddNewItem(itemsContainer: groupModels,
                    item: dialogResult.Result);
            }
        });

        ExcludeFileCommand = ReactiveCommand.CreateFromTask<ItemState>(execute: async item =>
        {
            var dialog = new DeleteDialogViewModel(
                message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item"
                , caption: "");

            if ((await RoutableViewModel.NavigateDialogAsync(dialog: dialog,
                    target: NavigationTarget.CompactDialogScreen)).Result)
            {
                _solutionStore.CurrentOpenSolution.DeleteItem(item: item);

                await _commandDispatcher.Dispatch<DeleteProjectFileItemCommand, Resource<Empty>>(
                    command: new DeleteProjectFileItemCommand(Item: new FileItem
                        { Name = item.Name, Path = item.ItemPath })
                    , cancellation: GetCancellationToken());
            }
        });

        CreateFolderCommand = ReactiveCommand.Create(execute: () =>
        {
            if (_solutionStore?.CurrentOpenSolution is not null)
            {
                _solutionStore?.CurrentOpenSolution?.AddNewFolderItem(item: new ItemGroupState
                {
                    InEditMode = true
                });
            }
        });

        CommitFolderCommand = ReactiveCommand.CreateFromTask<ItemGroupState?>(execute: async itemsGroup =>
        {
            if (itemsGroup is not null && _solutionStore?.CurrentOpenSolution is not null)
            {
                itemsGroup.ItemPath = Path.Combine(
                    path1: Directory.GetParent(path: _solutionStore?.CurrentOpenSolution?.FilePath).FullName,
                    path2: Constants.AppProjectItemsFolderName, path3: itemsGroup.Name);
            }

            await _commandDispatcher.Dispatch<CreateSolutionItemFolderCommand, Empty>(
                command: new CreateSolutionItemFolderCommand(
                    ItemName: itemsGroup.Name,
                    ItemPath: itemsGroup.ItemPath)
                , cancellation: GetCancellationToken());
        });

        CreateItemCommand = ReactiveCommand.CreateFromTask<ItemState?>(execute: async item =>
        {
            if (item is not null)
            {
                _editingItemsStore.Item = await _editingItemsStore.EditItem(item: item);
            }
        });
    }

    public SolutionState CurrentOpenSolution => _solutionStore?.CurrentOpenSolution;

    public ICommand CreateItemCommand
    {
        get;
    }

    public ICommand ExcludeFileCommand
    {
        get;
        set;
    }

    public ICommand RenameFileCommand
    {
        get;
        set;
    }

    public ICommand ExcludeFolderCommand
    {
        get;
        set;
    }

    public ICommand AddProjectItemCommand
    {
        get;
        set;
    }

    public ICommand CreateFolderCommand
    {
        get;
        set;
    }

    public ICommand CommitFolderCommand
    {
        get;
        set;
    }

    public ICommand OpenSolutionCommand
    {
        get;
        set;
    }
}