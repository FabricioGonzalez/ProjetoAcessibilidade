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
using Project.Domain.Contracts;
using Project.Domain.Solution.Queries;
using ProjectAvalonia.Features.Project.States;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.Features.Project.ViewModels;
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
                        CurrentOpenSolution = result?.Data?.ToSolutionState();
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

    [AutoNotify] private SolutionReportState _reportData;

    private SolutionState(string filePath, IList<ItemGroupState> itemsGroups, SolutionReportState reportData)
    {
        FilePath = filePath;
        FileName = Path.GetFileNameWithoutExtension(path: filePath);

        LoadAllItems(items: itemsGroups);
        ReportData = reportData;
    }

    public static SolutionState Create(string filePath, IList<ItemGroupState> itemsGroups,
        SolutionReportState reportData) => new(filePath: filePath, itemsGroups: itemsGroups, reportData: reportData);

    private void LoadAllItems(IList<ItemGroupState> items)
    {
        _itemsCollection = new SourceList<ItemGroupState>();

        _itemsCollection
            .Connect()
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Bind(readOnlyObservableCollection: out _itemGroups)
            .Subscribe();

        foreach (var item in items)
        {
            if (_itemsCollection.Items.All(predicate: i => i.ItemPath != item.ItemPath))
            {
                _itemsCollection.Add(item: item);
            }
        }
    }
}