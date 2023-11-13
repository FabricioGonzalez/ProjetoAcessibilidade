using System;
using DynamicData;
using ProjectAvalonia.Features.SearchBar.Patterns;
using ProjectAvalonia.Features.SearchBar.SearchItems;

namespace ProjectAvalonia.Features.SearchBar.Sources;

public interface ISearchSource
{
    IObservable<IChangeSet<ISearchItem, ComposedKey>> Changes
    {
        get;
    }
}