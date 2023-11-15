using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace ProjectAvalonia.Common.Converters;

public class IndentConverter
    : AvaloniaObject
        , IValueConverter
{
    public double Multiplier
    {
        get;
        set;
    }

    public object Convert(
        object? value
        , Type targetType
        , object? parameter
        , CultureInfo culture
    )
    {
        if (value is int indent)
        {
            return new Thickness(left: indent * Multiplier, top: 0, right: 0, bottom: 0);
        }

        return new Thickness();
    }

    public object ConvertBack(
        object? value
        , Type targetType
        , object? parameter
        , CultureInfo culture
    ) => throw new NotSupportedException();
}