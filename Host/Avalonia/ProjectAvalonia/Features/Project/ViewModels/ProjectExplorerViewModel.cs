﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common;

using Common.Linq;
using Common.Optional;

using DynamicData;
using DynamicData.Alias;
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
using ProjetoAcessibilidade.Domain.Solution.Commands.SolutionItem;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ProjectExplorerViewModel : ViewModelBase, IProjectExplorerViewModel
{
    private readonly IMediator _mediator;

    public ProjectExplorerViewModel(ProjectSolutionModel state)
    {
        SolutionState = state;

        _mediator = Locator.Current.GetService<IMediator>()!;

        Items = new ObservableCollection<IItemGroupViewModel>(list:
           SolutionState.ItemGroups
                .Select(
                 selector: model =>
                 {
                     var vm = new ItemGroupViewModel(
                     name: model.Name,
                     itemPath: model.ItemPath);
                     vm.TransformFrom(items: model.Items);

                     return vm;
                 }).ToList<IItemGroupViewModel>());

        var changeSet = Items
            .ToObservableChangeSet();

        changeSet.AutoRefreshOnObservable(reevaluator: document => document.AddProjectItemCommand.IsExecuting)
            .Select(selector: x => WhenAnyItemWasAdded())
            .Switch()
             .SubscribeAsync(async item =>
             {
                 SolutionState.ReloadItem(Items
                     .Select(x => new ItemGroupModel()
                     {
                         Name = x.Name,
                         ItemPath = x.ItemPath,
                         Items = x.Items.Select(x => new ItemModel()
                         {
                             Id = x.Id,
                             ItemPath = x.ItemPath,
                             Name = x.Name,
                             TemplateName = x.TemplateName
                         })
                    .ToList()
                     }).ToList());

                 await SaveSolution();
             });

        changeSet
            .AutoRefreshOnObservable(reevaluator: document => document.ExcludeFolderCommand.IsExecuting)
            .Select(selector: x => WhenAnyFolderIsDeleted())
            .Switch()
            .SubscribeAsync(onNextAsync: async x =>
            {
                var dialog = new DeleteDialogViewModel(
                    message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item"
                    , caption: "");

                if ((await RoutableViewModel.NavigateDialogAsync(dialog: dialog,
                        target: NavigationTarget.CompactDialogScreen)).Result)
                {
                    Items.Remove(item: x);
                    SolutionState.RemoveFromSolution(i => i.Name == x.Name);
                }
            });

        CreateFolderCommand = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var dialog = new CreateFolderViewModel(
                message: "Defina o nome da pasta", title: "Criar Pasta"
                , caption: "",
                Items);

            var result = await RoutableViewModel.NavigateDialogAsync(dialog: dialog,
                target: NavigationTarget.CompactDialogScreen);

            if (result.Kind == DialogResultKind.Normal)
            {

                var item = new ItemGroupViewModel(name: result.Result, itemPath: Path.Combine(Directory.GetParent(SolutionState.FilePath).FullName, Constants.AppProjectItemsFolderName, result.Result));

                Items.Add(item: item);

                SolutionState.AddItemToSolution(new()
                {
                    Name = result.Result,
                    ItemPath = item.ItemPath,
                    Items = item.Items.Select(x => new ItemModel()
                    {
                        Id = x.Id,
                        ItemPath = x.ItemPath,
                        Name = x.Name,
                        TemplateName = x.TemplateName
                    })
                    .ToList()
                });

                await _mediator.Send(new CreateSolutionItemFolderCommand(item.Name, item.ItemPath), CancellationToken.None);

                Items.Add(item: item);

                return Optional<IItemGroupViewModel>.Some(item);
            }
            return Optional<IItemGroupViewModel>.None();
        });

        CreateFolderCommand.Subscribe(result =>
        {
            result.Map(res =>
            {
                state.ItemGroups.AddIfNotFound((ItemGroupModel)res, (i) => i.Name != res.Name);

                return res;
            });
        });

        CreateFolderCommand
            .DoAsync(async _ => await SaveSolution())
            .Subscribe();
    }

    private IObservable<IItemViewModel?> WhenAnyItemWasAdded() =>
    // Select the documents into a list of Observables
    // who return the Document to close when signaled,
    // then flatten them all together.
    Items
        .Select(selector: x => x.AddProjectItemCommand.Select(s => s))
        .Merge();


    private async Task SaveSolution()
    {
        if (SolutionState is not null)
        {
            await _mediator.Send(
                request: new CreateSolutionCommand(
                    SolutionPath: SolutionState.FilePath,
                    SolutionData: SolutionState),
                cancellation: CancellationToken.None);
        }
    }

    public ObservableCollection<IItemGroupViewModel> Items
    {
        get;
        set;
    }

    public void SetCurrentSolution(ProjectSolutionModel state)
    {
        SolutionState = state;
    }
    public ReactiveCommand<Unit, Optional<IItemGroupViewModel>> CreateFolderCommand
    {
        get;
    }

    public ProjectSolutionModel SolutionState
    {
        get; private set;
    }

    private IObservable<IItemGroupViewModel?> WhenAnyFolderIsDeleted() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        Items
            .Select(selector: x => x.ExcludeFolderCommand.Select(selector: _ => x))
            .Merge();
}