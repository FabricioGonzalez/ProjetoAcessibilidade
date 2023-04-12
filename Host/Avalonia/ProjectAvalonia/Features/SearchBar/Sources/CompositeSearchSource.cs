using System;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using ProjectAvalonia.Features.SearchBar.Patterns;
using ProjectAvalonia.Features.SearchBar.SearchItems;
using ProjectAvalonia.Features.SearchBars.ViewModels.SearchBar.Sources;

namespace ProjectAvalonia.Features.SearchBar.Sources;

public class CompositeSearchSource : ISearchSource
{
    private readonly ISearchSource[] _sources;

    public CompositeSearchSource(
        params ISearchSource[] sources
    )
    {
        _sources = sources;
    }

    public IObservable<IChangeSet<ISearchItem, ComposedKey>> Changes =>
        _sources.Select(selector: r => r.Changes).Merge();
}