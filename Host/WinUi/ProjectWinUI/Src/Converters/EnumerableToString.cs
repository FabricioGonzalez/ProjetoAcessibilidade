using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.UI.Xaml.Data;

namespace ProjectWinUI.Src.Converters;

public class EnumerableToString : IValueConverter
{
    public object Convert(
        object value
        , Type targetType
        , object parameter
        , string language
    )
    {
        var data = "";

        if (value is not null)
        {
            var sb = new StringBuilder();

            foreach (var item in (IEnumerable<string>)value)
            {
                sb.AppendLine(value: item);
            }

            data = sb.ToString();

            return data;
        }

        return null;
    }

    public object ConvertBack(
        object value
        , Type targetType
        , object parameter
        , string language
    )
    {
        var data = "";

        if (value is not null)
        {
            var sb = new StringBuilder();

            foreach (var item in (IEnumerable<string>)value)
            {
                sb.AppendLine(value: item);
            }

            data = sb.ToString();

            return data;
        }

        return null;
    }
}