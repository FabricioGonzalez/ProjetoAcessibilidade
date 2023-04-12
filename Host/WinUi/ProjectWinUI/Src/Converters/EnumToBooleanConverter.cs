using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace ProjectWinUI.Src.Converters;

public class EnumToBooleanConverter : IValueConverter
{
    public object Convert(
        object value
        , Type targetType
        , object parameter
        , string language
    )
    {
        if (parameter is string enumString)
        {
            if (!Enum.IsDefined(enumType: typeof(ElementTheme), value: value))
            {
                throw new ArgumentException(message: "ExceptionEnumToBooleanConverterValueMustBeAnEnum");
            }

            var enumValue = Enum.Parse(enumType: typeof(ElementTheme), value: enumString);

            return enumValue.Equals(obj: value);
        }

        throw new ArgumentException(message: "ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
    }

    public object ConvertBack(
        object value
        , Type targetType
        , object parameter
        , string language
    )
    {
        if (parameter is string enumString)
        {
            return Enum.Parse(enumType: typeof(ElementTheme), value: enumString);
        }

        throw new ArgumentException(message: "ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
    }
}