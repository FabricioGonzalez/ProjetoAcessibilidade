﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace ProjetoAcessibilidade.Helpers;
public class VisibilityToNullConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not null)
        {
            if (value is string str && str.Length == 0)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (Visibility.Collapsed == (Visibility)value)
        {
            return null;
        }
        return "";
    }
}
