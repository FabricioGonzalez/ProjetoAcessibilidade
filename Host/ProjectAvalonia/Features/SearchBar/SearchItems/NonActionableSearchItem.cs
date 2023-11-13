using System.Collections.Generic;
using ProjectAvalonia.Features.SearchBar.Patterns;

namespace ProjectAvalonia.Features.SearchBar.SearchItems;

public class NonActionableSearchItem : ISearchItem
{
    public NonActionableSearchItem(
        object content
        , string name
        , string category
        , IEnumerable<string> keywords
        , string? icon
    )
    {
        Name = name;
        Content = content;
        Category = category;
        Keywords = keywords;
        Icon = icon;
    }

    public object Content
    {
        get;
    }

    public string Name
    {
        get;
    }

    public ComposedKey Key => new(keys: Name);
    public string Description => "";

    public string? Icon
    {
        get;
        set;
    }

    public string Category
    {
        get;
    }

    public IEnumerable<string> Keywords
    {
        get;
    }

    public bool IsDefault
    {
        get;
        set;
    }
}