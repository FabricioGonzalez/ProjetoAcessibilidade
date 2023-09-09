using System;
using System.Globalization;

using Avalonia.Data.Converters;

namespace ProjectAvalonia.Common.Converters;
public class SizePercentageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null ? int.Parse(value.ToString() ?? 0.ToString()) * 100 / double.Parse((string)parameter) : 0;
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null ? (int.Parse(value.ToString() ?? 0.ToString()) / double.Parse((string)parameter)) * 100 : 0;
    }
}
