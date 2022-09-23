using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace AppWinui.AppCode.AppUtils.Helpers;
public class NotNullToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not null)
        {
            if (value is string str && str.Length == 0)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (Visibility.Collapsed == (Visibility)value)
        {
            return "";
        }
        return null;
    }
}
