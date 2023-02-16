using System;
using System.Globalization;

using Avalonia;
using Avalonia.Data.Converters;

using ProjectAvalonia.Common.Extensions;

namespace ProjectAvalonia.Common.Converters;

public static class EnumConverters
{
    public static readonly IValueConverter ToFriendlyName =
        new FuncValueConverter<Enum, object>(x => x?.FriendlyName() ?? AvaloniaProperty.UnsetValue);

    public static readonly IValueConverter ToUpperCase =
        new FuncValueConverter<Enum, object>(x => x?.ToString().ToUpper(CultureInfo.InvariantCulture) ?? AvaloniaProperty.UnsetValue);
}
