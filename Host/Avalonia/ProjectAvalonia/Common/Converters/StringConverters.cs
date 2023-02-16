using Avalonia.Data.Converters;

namespace ProjectAvalonia.Common.Converters;

public static class StringConverters
{
    public static readonly IValueConverter ToUpperCase =
        new FuncValueConverter<string?, string?>(x => x?.ToUpperInvariant());
}
