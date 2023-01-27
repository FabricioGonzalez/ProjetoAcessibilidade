using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Binding;
using DynamicData.PLinq;

using ProjectAvalonia.Features.SearchBar.Patterns;
using ProjectAvalonia.Features.SearchBar.SearchItems;

using ReactiveUI;

namespace ProjectAvalonia.Features.SearchBar;

public partial class SearchBarViewModel : ReactiveObject
{
    private readonly ReadOnlyObservableCollection<SearchItemGroup> _groups;
    [AutoNotify] private bool _isSearchListVisible;
    [AutoNotify] private string _searchText = "";

    public SearchBarViewModel(IObservable<IChangeSet<ISearchItem, ComposedKey>> itemsObservable)
    {
        itemsObservable
            .Group(s => s.Category)
            .Transform(group => new SearchItemGroup(group.Key, group.Cache.Connect()))
            .Sort(SortExpressionComparer<SearchItemGroup>.Ascending(x => x.Title))
            .Bind(out _groups)
            .DisposeMany()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe();

        HasResults = itemsObservable
            .Count()
            .Select(i => i > 0)
            .Replay(1)
            .RefCount();
    }

    public IObservable<bool> HasResults
    {
        get;
    }

    public ReadOnlyObservableCollection<SearchItemGroup> Groups => _groups;
}
