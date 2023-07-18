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
        if (Application.Current is not null && value is string iconName)
        {
            if (Application.Current.Styles.TryGetResource(key: iconName
                    , theme: Application.Current.RequestedThemeVariant, value: out var resource))
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