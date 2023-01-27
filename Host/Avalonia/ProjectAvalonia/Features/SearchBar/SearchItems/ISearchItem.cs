using System.Collections.Generic;

using ProjectAvalonia.Features.SearchBar.Patterns;

namespace ProjectAvalonia.Features.SearchBar.SearchItems;

public interface ISearchItem
{
    string Name
    {
        get;
    }
    string Description
    {
        get;
    }
    ComposedKey Key
    {
        get;
    }
    string? Icon
    {
        get; set;
    }
    string Category
    {
        get;
    }
    IEnumerable<string> Keywords
    {
        get;
    }
    bool IsDefault
    {
        get;
    }
}
