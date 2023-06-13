using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Linq;
public static class LinqExtensions
{
    public static IEnumerable<T> AddIfNotFound<T>(this IEnumerable<T> items, T target,Func<T,bool> predicate)
    {
        var exists = items.Any(predicate);

        return !exists ? items.Append(target) : items; 
    }
}
