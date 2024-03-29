using Avalonia.Data.Converters;

namespace ProjectAvalonia.Common.Converters;

public static class BoolOpacityConverters
{
    public static readonly IValueConverter BoolToOpacity =
        new FuncValueConverter<bool, double>(convert: x => x ? 1.0 : 0.0);

    public static readonly IValueConverter OpacityToBool =
        new FuncValueConverter<double, bool>(convert: x => x > 0);
}