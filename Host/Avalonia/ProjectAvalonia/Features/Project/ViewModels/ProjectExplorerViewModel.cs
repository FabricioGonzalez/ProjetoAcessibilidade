using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Optional;
using DynamicData;
using DynamicData.Binding;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Commands.FolderItems;
using ProjetoAcessibilidade.Domain.Solution.Commands.SolutionItem;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ProjectExplorerViewModel
    : ViewModelBase
        , IProjectExplorerViewModel
{
    private readonly IMediator _mediator;
    private IItemViewModel _selectedItem;
    private ISolutionGroupViewModel _solutionGroupView;

    public ProjectExplorerViewModel(
        ProjectSolutionModel state
    )
    {
        SolutionState = state;

        _mediator = Locator.Current.GetService<IMediator>()!;

        SolutionRootItem = new SolutionGroupViewModel();

        SolutionRootItem.SolutionItem = new SolutionItemViewModel("", state.FilePath
            , Path.GetFileNameWithoutExtension(state.FilePath), "");

        SolutionRootItem.ConclusionItem = new ConclusionItemViewModel(Guid.NewGuid().ToString()
            , Path.Combine(Path.GetDirectoryName(state.FilePath), "conclusion.prjc"), "Conclusão", "");

        SolutionRootItem.ItemsGroups = new ObservableCollection<IItemGroupViewModel>(SolutionState.ItemGroups
            .Select(
                model =>
                {
                    var vm = new ItemGroupViewModel(
                        model.Name,
                        model.ItemPath,
                        async () => await SaveSolution());

                    vm.SelectItemToEdit = SetEditingItem;

                    vm.MoveItemCommand = ReactiveCommand.CreateFromTask(SaveSolution);
                    vm.TransformFrom(model.Items);

                    return vm;
                }).ToList<IItemGroupViewModel>());

        this.WhenAnyValue(vm => vm.SelectedItem)
            .WhereNotNull()
            .Subscribe(it =>
            {
                Debug.WriteLine(it.Name);
            });

        var changeSet = SolutionRootItem.ItemsGroups
            .ToObservableChangeSet();

        _ = changeSet.AutoRefreshOnObservable(document => document.AddProjectItemCommand.IsExecuting)
            .Select(x => WhenAnyItemWasAdded())
            .Switch()
            .SubscribeAsync(async item =>
            {
                await SaveSolution();
            });

        _ = changeSet
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
                    _ = SolutionRootItem.ItemsGroups.Remove(x);

                    _ = await _mediator.Send(new DeleteProjectFolderItemCommand(x.ItemPath), CancellationToken.None);

                    SolutionState.RemoveFromSolution(i => i.Name == x.Name);

                    await SaveSolution();
                }
            });

        CreateFolderCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var dialog = new CreateFolderViewModel(
                "Defina o nome da pasta", "Criar Pasta"
                , "",
                SolutionRootItem.ItemsGroups);

            var result = await RoutableViewModel.NavigateDialogAsync(dialog,
                NavigationTarget.CompactDialogScreen);

            if (result.Kind == DialogResultKind.Normal)
            {
                var item = new ItemGroupViewModel(
                    result.Result,
                    Path.Combine(Directory.GetParent(SolutionState.FilePath).FullName
                        , Constants.AppProjectItemsFolderName, result.Result),
                    async () => await SaveSolution());

                SolutionRootItem.ItemsGroups.Add(item);

                SolutionState.AddItemToSolution(new ItemGroupModel
                {
                    Name = result.Result, ItemPath = item.ItemPath, Items = item.Items.Select(x => new ItemModel
                        {
                            Id = x.Id, ItemPath = x.ItemPath, Name = x.Name, TemplateName = x.TemplateName
                        })
                        .ToList()
                });

                _ = await _mediator.Send(new CreateSolutionItemFolderCommand(item.Name, item.ItemPath)
                    , CancellationToken.None);

                return Optional<IItemGroupViewModel>.Some(item);
            }

            return Optional<IItemGroupViewModel>.None();
        });

        _ = CreateFolderCommand
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
        set => this.RaiseAndSetIfChanged(ref _solutionGroupView, value);
    }

    public IItemViewModel SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public void SetCurrentSolution(
        ProjectSolutionModel state
    ) => SolutionState = state;

    public ReactiveCommand<Unit, Optional<IItemGroupViewModel>> CreateFolderCommand
    {
        get;
    }

    public ProjectSolutionModel SolutionState
    {
        get;
        private set;
    }

    private IObservable<IItemViewModel?> WhenAnyItemWasAdded() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        SolutionRootItem.ItemsGroups
            .Select(x => x.AddProjectItemCommand.Select(s => s))
            .Merge();

    private IObservable<IItemGroupViewModel?> WhenAnyItemWasMoved() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        SolutionRootItem.ItemsGroups
            .Select(x => x.MoveItemCommand.Select(_ => x))
            .Merge();


    private async Task SaveSolution()
    {
        SolutionState?.ReloadItem(SolutionRootItem.ItemsGroups
            .Select(x => new ItemGroupModel
            {
                Name = x.Name, ItemPath = x.ItemPath, Items = x.Items.Select(x => new ItemModel
                    {
                        Id = x.Id, ItemPath = x.ItemPath, Name = x.Name, TemplateName = x.TemplateName
                    })
                    .ToList()
            }).ToList());

        _ = await _mediator.Send(
            new CreateSolutionCommand(
                SolutionState?.FilePath,
                SolutionState),
            CancellationToken.None);
    }

    private IObservable<IItemGroupViewModel?> WhenAnyFolderIsDeleted() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        SolutionRootItem.ItemsGroups
            .Select(x => x.ExcludeFolderCommand.Select(_ => x))
            .Merge();
}