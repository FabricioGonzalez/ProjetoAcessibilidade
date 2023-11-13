using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ProjectAvalonia.Common.Converters;

public class PhoneMaskConverter : IValueConverter
{
    public object? Convert(
        object? value
        , Type targetType
        , object? parameter
        , CultureInfo culture
    )
    {
        if (value is string val && parameter is string par)
        {
            if (val
                    .Length >= 14)
            {
                return "(00)00000-0000";
            }
        }

        return "(00)0000-0000";
    }

    public object? ConvertBack(
        object? value
        , Type targetType
        , object? parameter
        , CultureInfo culture
    ) => "(00)0000-0000";
}