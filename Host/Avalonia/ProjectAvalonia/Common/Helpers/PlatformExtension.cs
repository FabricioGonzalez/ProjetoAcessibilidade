using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Data.Core;
using Avalonia.Markup.Xaml;
using Avalonia.Utilities;

namespace ProjectAvalonia.Common.Helpers;

public class PlatformExtension : MarkupExtension
{
    public PlatformExtension()
    {
    }

    public PlatformExtension(
        object defaultValue
    )
    {
        Default = defaultValue;
    }

    public object? Default
    {
        get;
        set;
    }

    public object? Osx
    {
        get;
        set;
    }

    public object? Linux
    {
        get;
        set;
    }

    public object? Windows
    {
        get;
        set;
    }

    public override object? ProvideValue(
        IServiceProvider serviceProvider
    )
    {
        var result = Default;

        if (Osx is not null && RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.OSX))
        {
            result = Osx;
        }
        else if (Linux is not null && RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Linux))
        {
            result = Linux;
        }
        else if (Windows is not null && RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows))
        {
            result = Windows;
        }

        var provideValueTarget =
            serviceProvider.GetService(serviceType: typeof(IProvideValueTarget)) as IProvideValueTarget;

        if (provideValueTarget is { TargetProperty: IPropertyInfo propertyInfo })
        {
            if (TypeUtilities.TryConvert(
                    to: propertyInfo.PropertyType,
                    value: result,
                    culture: CultureInfo.InvariantCulture,
                    result: out var converted))
            {
                return converted;
            }
        }

        return AvaloniaProperty.UnsetValue;
    }
}