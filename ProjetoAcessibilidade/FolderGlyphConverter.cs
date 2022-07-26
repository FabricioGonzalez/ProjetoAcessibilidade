using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml.Data;

namespace ProjetoAcessibilidade;
public class FolderGlyphConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is bool val)
        {
            return val ? "&#xED43;" : "&#xED41;";
        }
        return "&#xED43;";
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if(value is string glyph)
        {
            if (glyph == "&#xED43;")
                return true;

            return false;
        }
        return false;
    }
}
