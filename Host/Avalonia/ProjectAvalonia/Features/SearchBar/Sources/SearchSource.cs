using System;
using System.Linq;

using ProjectAvalonia.Features.SearchBar.SearchItems;

namespace ProjectAvalonia.Features.SearchBar.Sources;

public static class SearchSource
{
    public static Func<ISearchItem, bool> DefaultFilter(string query)
    {
        return item =>
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return item.IsDefault;
            }

            return new[] { item.Name, item.Description, }.Concat(item.Keywords)
                .Any(s => s.Contains(query, StringComparison.InvariantCultureIgnoreCase));
        };
    }
}
