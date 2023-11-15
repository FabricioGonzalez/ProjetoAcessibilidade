using Avalonia;
using Avalonia.Data.Converters;

namespace ProjectAvalonia.Common.Converters;

public class ThicknessConverters
{
    public static readonly IValueConverter Negate =
        new FuncValueConverter<Thickness, Thickness>(convert: thickness =>
            new Thickness(left: -thickness.Left, top: -thickness.Top, right: -thickness.Right
                , bottom: -thickness.Bottom));
}