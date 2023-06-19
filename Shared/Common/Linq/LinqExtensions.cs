namespace Common.Linq;
public static class LinqExtensions
{
    public static IEnumerable<T> AddIfNotFound<T>(this IEnumerable<T> items, T target, Func<T, bool> predicate)
    {
        var exists = items.Any(predicate);

        return !exists ? items.Append(target) : items;
    }

    public static IEnumerable<T> IterateOn<T>(this IEnumerable<T> items, Action<T> iterator)
    {
        foreach (var item in items)
        {
            iterator(item);
        }
        return items;
    }
}
