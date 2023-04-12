using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace ProjectAvalonia.Common.Converters;

public class BoolToGenericConverter<T>
    : AvaloniaObject
        , IValueConverter
{
    public static readonly StyledProperty<T?> TrueProperty =
        AvaloniaProperty.Register<BoolToGenericConverter<T>, T?>(name: nameof(True));

    public static readonly StyledProperty<T?> FalseProperty =
        AvaloniaProperty.Register<BoolToGenericConverter<T>, T?>(name: nameof(False));

    public T? True
    {
        get => GetValue(property: TrueProperty);
        set => SetValue(property: TrueProperty, value: value);
    }

    public T? False
    {
        get => GetValue(property: FalseProperty);
        set => SetValue(property: FalseProperty, value: value);
    }

    public object? Convert(
        object? value
        , Type targetType
        , object? parameter
        , CultureInfo culture
    )
    {
        if (value is true)
        {
            return True ?? AvaloniaProperty.UnsetValue;
        }

        return False ?? AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(
        object? value
        , Type targetType
        , object? parameter
        , CultureInfo culture
    ) => throw new NotImplementedException();
}