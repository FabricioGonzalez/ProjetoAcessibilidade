using System;
using System.Globalization;

using Avalonia.Data.Converters;

using ProjectAvalonia.Models.ValidationTypes;
using ProjectAvalonia.Presentation.Enums;

namespace ProjectAvalonia.Common.Converters;
public class TypeToCheckingTypeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {

        if (AppValidation.GetCheckingRuleTypeByValue((AppFormDataType)value) is { } result)
            return result;

        return null;

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
       (value as ICheckingRuleTypes)?.Value;
}