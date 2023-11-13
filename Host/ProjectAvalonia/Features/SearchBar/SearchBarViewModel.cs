using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Binding;
using ProjectAvalonia.Features.SearchBar.Patterns;
using ProjectAvalonia.Features.SearchBar.SearchItems;
using ReactiveUI;

namespace ProjectAvalonia.Features.SearchBar;

public partial class SearchBarViewModel : ReactiveObject
{
    private readonly ReadOnlyObservableCollection<SearchItemGroup> _groups;
    [AutoNotify] private bool _isSearchListVisible;
    [AutoNotify] private string _searchText = "";

    public SearchBarViewModel(
        IObservable<IChangeSet<ISearchItem, ComposedKey>> itemsObservable
    )
    {
        itemsObservable
            .Group(groupSelectorKey: s => s.Category)
            .Transform(transformFactory: group => new SearchItemGroup(title: group.Key, changes: group.Cache.Connect()))
            .Sort(comparer: SortExpressionComparer<SearchItemGroup>.Ascending(expression: x => x.Title))
            .Bind(readOnlyObservableCollection: out _groups)
            .DisposeMany()
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Subscribe();

        HasResults = itemsObservable
            .Count()
            .Select(selector: i => i > 0)
            .Replay(bufferSize: 1)
            .RefCount();
    }

    public IObservable<bool> HasResults
    {
        get;
    }

    public ReadOnlyObservableCollection<SearchItemGroup> Groups => _groups;
}