using System;
using System.Linq;
using ProjectAvalonia.Features.SearchBar.SearchItems;

namespace ProjectAvalonia.Features.SearchBar.Sources;

public static class SearchSource
{
    public static Func<ISearchItem, bool> DefaultFilter(
        string query
    ) =>
        item =>
        {
            if (string.IsNullOrWhiteSpace(value: query))
            {
                return item.IsDefault;
            }

            return new[] { item.Name, item.Description }.Concat(second: item.Keywords)
                .Any(predicate: s =>
                    s.Contains(value: query, comparisonType: StringComparison.InvariantCultureIgnoreCase));
        };
}