using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace ProjetoAcessibilidade.Helpers;
public class NegativeBoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not null)
        {
            return (bool)value == false ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is not null)
        {
            Enum.TryParse<Visibility>(value.ToString(), out var val);

            return val == Visibility.Collapsed;
        }
        return true;
    }
}
