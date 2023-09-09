using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ProjectAvalonia.Common.Converters;

public class NavBarIconConverter : IValueConverter
{
    public static readonly NavBarIconConverter Instance = new();

    private NavBarIconConverter()
    {
    }

    object IValueConverter.Convert(
        object? value
        , Type targetType
        , object? parameter
        , CultureInfo culture
    )
    {
        if (Avalonia.Application.Current is not null && value is string iconName)
        {
            if (Avalonia.Application.Current.Styles.TryGetResource(key: iconName
                    , theme: Avalonia.Application.Current.RequestedThemeVariant, value: out var resource))
            {
                return resource is not StreamGeometry ? AvaloniaProperty.UnsetValue : resource;
            }
        }

        return AvaloniaProperty.UnsetValue;
    }

    object IValueConverter.ConvertBack(
        object? value
        , Type targetType
        , object? parameter
        , CultureInfo culture
    ) => throw new NotImplementedException();
}