using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ProjectAvalonia.Features.SearchBar.Patterns;
using ProjectAvalonia.Features.SearchBar.SearchItems;
using ReactiveUI;

namespace ProjectAvalonia.Features.SearchBar;

public class SearchItemGroup : IDisposable
{
    private readonly CompositeDisposable _disposables = new();
    private readonly ReadOnlyObservableCollection<ISearchItem> _items;

    public SearchItemGroup(
        string title
        , IObservable<IChangeSet<ISearchItem, ComposedKey>> changes
    )
    {
        Title = title;
        changes
            .Bind(readOnlyObservableCollection: out _items)
            .DisposeMany()
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Subscribe()
            .DisposeWith(compositeDisposable: _disposables);
    }

    public string Title
    {
        get;
    }

    public ReadOnlyObservableCollection<ISearchItem> Items => _items;

    public void Dispose() => _disposables.Dispose();
}