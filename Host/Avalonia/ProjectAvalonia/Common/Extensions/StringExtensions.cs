using System;

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