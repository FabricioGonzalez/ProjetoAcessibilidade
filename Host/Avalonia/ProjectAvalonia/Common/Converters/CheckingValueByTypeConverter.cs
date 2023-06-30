using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ProjectAvalonia.Models.ValidationTypes;

namespace ProjectAvalonia.Common.Converters;

public class CheckingValueByTypeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is string s && !string.IsNullOrWhiteSpace(s) &&
        AppValidation.GetCheckingOperationByValue(s) is IsOperation;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is string s && !string.IsNullOrWhiteSpace(s) &&
        AppValidation.GetCheckingOperationByValue(s) is not IsOperation;
}