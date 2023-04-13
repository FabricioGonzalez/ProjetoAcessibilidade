using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Common;
using Core.Entities.Solution;
using DynamicData;
using DynamicData.Binding;
using Project.Domain.Contracts;
using Project.Domain.Solution.Queries;
using ProjectAvalonia.Common.Mappers;
using ProjectAvalonia.Features.Project.States;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.Logging;
using ReactiveUI;

namespace ProjectAvalonia.Stores;

public partial class SolutionStore
{
    private readonly IQueryDispatcher _queryDispatcher;
    [AutoNotify] private SolutionState? _currentOpenSolution;
    [AutoNotify] private SourceList<SolutionState>? _recentOpenedSolutions;

    public SolutionStore(IQueryDispatcher queryDispatcher)
    {
        _queryDispatcher = queryDispatcher;
    }

    public void CreateSolutionState(string solutionPath) => CurrentOpenSolution =
        SolutionState.Create(filePath: solutionPath, itemsGroups: new List<ItemGroupState>(),
            reportData: new SolutionReportState());

    public async Task LoadSolution(string solutionPath) =>
        (await _queryDispatcher.Dispatch<ReadSolutionProjectQuery, Resource<ProjectSolutionModel>>(
            query: new ReadSolutionProjectQuery(SolutionPath: solutionPath),
            cancellation: CancellationToken.None))
        .OnSuccess(
            onSuccessAction: result =>
            {
                Dispatcher
                    .UIThread
                    .Post(action: () =>
                    {
                        CurrentOpenSolution = result?.Data.ToSolutionState();
                    });
            })
        .OnError(onErrorAction: error =>
        {
        });
}

public partial class SolutionState
{
    [AutoNotify] private string _fileName = "";

    [AutoNotify] private string _filePath = "";

    [AutoNotify] private ReadOnlyObservableCollection<ItemGroupState> _itemGroups;
    private SourceList<ItemGroupState> _itemsCollection;
    [AutoNotify] private string _logoPath = "";

    [AutoNotify] private SolutionReportState _reportData;

    private SolutionState(string filePath, IList<ItemGroupState> itemsGroups, SolutionReportState reportData)
    {
        FilePath = filePath;
        FileName = Path.GetFileNameWithoutExtension(path: filePath);

        LoadAllItems(items: itemsGroups);

        _itemsCollection
            .Connect()
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Bind(readOnlyObservableCollection: out _itemGroups)
            .Subscribe();

        _itemsCollection
            .Connect()
            .ActOnEveryObject(onAdd: item =>
                {
                    Logger.LogDebug(message: $"item {item.Name} was added");
                },
                onRemove: item =>
                {
                    Logger.LogDebug(message: $"item {item.Name} was removed");
                });

        _itemsCollection
            .Items
            .SelectMany(selector: x => x.Items)
            .AsObservableChangeSet()
            .ActOnEveryObject(onAdd: item =>
                {
                    Logger.LogDebug(message: $"project item {item.Name} was added");
                },
                onRemove: item =>
                {
                    Logger.LogDebug(message: $"project item {item.Name} was removed");
                });

        ReportData = reportData;
    }

    public static SolutionState Create(string filePath, IList<ItemGroupState> itemsGroups,
        SolutionReportState reportData) => new(filePath: filePath, itemsGroups: itemsGroups, reportData: reportData);

    public void DeleteFolderItem(ItemGroupState item) =>
        _itemsCollection.Remove(item: item);

    public void DeleteItem(ItemState item)
    {
        foreach (var group in _itemsCollection.Items)
        {
            if (group.Items.FirstOrDefault(predicate: x => x.ItemPath == item.ItemPath) is { } itemToRemove)
            {
                group.Items.Remove(item: itemToRemove);
            }
        }
    }

    public void AddNewFolderItem(ItemGroupState item) => _itemsCollection.Add(item: item);

    public void AddNewItem(ItemGroupState itemsContainer, ItemState item)
    {
        if (_itemsCollection.Items.FirstOrDefault(predicate: x => x.Name == itemsContainer.Name) is { } group)
        {
            if (group.Items.All(predicate: i => i.Name != item.Name))
            {
                group.Items.Add(item: item);
            }
        }
    }

    private void LoadAllItems(IList<ItemGroupState> items) =>
        _itemsCollection =
            new SourceList<ItemGroupState>(source:
                new ObservableCollectionExtended<ItemGroupState>(collection: items)
                    .ToObservableChangeSet());
}