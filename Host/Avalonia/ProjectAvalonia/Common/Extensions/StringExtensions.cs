using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectAvalonia.Common.Extensions;

public static class StringExtensions
{
    /// <summary>
    ///     Removes one leading occurrence of the specified string
    /// </summary>
    public static string TrimStart(
        this string me
        , string trimString
        , StringComparison comparisonType
    )
    {
        if (me.StartsWith(value: trimString, comparisonType: comparisonType))
        {
            return me[trimString.Length..];
        }

        return me;
    }

    /// <summary>
    ///     Removes one trailing occurrence of the specified string
    /// </summary>
    public static string TrimEnd(
        this string me
        , string trimString
        , StringComparison comparisonType
    )
    {
        if (me.EndsWith(value: trimString, comparisonType: comparisonType))
        {
            return me[..^trimString.Length];
        }

        return me;
    }

    public static IEnumerable<string> SplitPath(this string value, Range range, string separator)
    {
        return value.Split(separator)[range];
    }
    public static IEnumerable<string> SplitPath(this string value, Range range, char separator)
    {
        return value.Split(separator)[range];
    }
    public static IEnumerable<string> SplitPath(this string value, string separator)
    {
        return value.Split(separator);
    }
    public static IEnumerable<string> SplitPath(this string value, char separator)
    {
        return value.Split(separator);
    }

    public static string JoinPath(this IEnumerable<string> value, string separator)
    {
        return string.Join(separator, value);
    }
    public static string JoinPath(this IEnumerable<string> value, char separator)
    {
        return string.Join(separator, value);
    }

    public static string GetFileNameWithoutExtension(this string value)
    {
        return Path.GetFileNameWithoutExtension(value);
    }

    public static string ToTitleCase(
        this string value
    )
    {
        if (string.IsNullOrWhiteSpace(value: value))
        {
            return value;
        }

        if (value.Length < 2)
        {
            return value.ToUpper();
        }

        return char.ToUpper(c: value[index: 0]) + value[1..];
    }

    /// <summary>
    ///     Returns true if the string contains leading or trailing whitespace, otherwise returns false.
    /// </summary>
    public static bool IsTrimmable(
        this string me
    )
    {
        if (me.Length == 0)
        {
            return false;
        }

        return char.IsWhiteSpace(c: me[index: 0]) || char.IsWhiteSpace(c: me[^1]);
    }
}