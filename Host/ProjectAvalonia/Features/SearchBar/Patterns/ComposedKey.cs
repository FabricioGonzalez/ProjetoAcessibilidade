using System.Collections.Generic;

namespace ProjectAvalonia.Features.SearchBar.Patterns;

public class ComposedKey : ValueObject
{
    public ComposedKey(
        params object[] keys
    )
    {
        Keys = keys;
    }

    public object[] Keys
    {
        get;
    }

    protected override IEnumerable<object> GetEqualityComponents() => Keys;
}