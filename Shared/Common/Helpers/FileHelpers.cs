using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers;
public static class FileHelpers
{
    public static string ExistsOrDefault(this string path, string defaultValue = "")
    {
        return File.Exists(path) ? path : defaultValue;
    }
}
