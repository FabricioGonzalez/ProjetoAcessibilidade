
using System.Text;

using Microsoft.UI.Xaml.Data;

namespace AppWinui.AppCode.AppUtils.Helpers;
public class EnumerableToString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        string data = "";

        if (value is not null)
        {
            var sb = new StringBuilder();

            foreach (var item in (IEnumerable<string>)value)
            {
                sb.AppendLine(item);
            }

            data = sb.ToString();

            return data;
        }
        return null;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        string data = "";

        if (value is not null)
        {
            var sb = new StringBuilder();

            foreach (var item in (IEnumerable<string>)value)
            {
                sb.AppendLine(item);
            }

            data = sb.ToString();

            return data;
        }
        return null;
    }
}
