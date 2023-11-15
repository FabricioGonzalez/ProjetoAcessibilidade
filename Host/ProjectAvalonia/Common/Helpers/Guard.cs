using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ProjectAvalonia.Common.Helpers;

public static class Guard
{
    public static bool True(
        string parameterName
        , bool? value
        , string? description = null
    )
        => AssertBool(parameterName: parameterName, expectedValue: true, value: value, description: description);

    public static bool False(
        string parameterName
        , bool? value
        , string? description = null
    )
        => AssertBool(parameterName: parameterName, expectedValue: false, value: value, description: description);

    private static bool AssertBool(
        string parameterName
        , bool expectedValue
        , bool? value
        , string? description = null
    )
    {
        NotNull(parameterName: parameterName, value: value);

        if (value != expectedValue)
        {
            throw new ArgumentOutOfRangeException(paramName: parameterName, actualValue: value
                , message: description ?? $"Parameter must be {expectedValue}.");
        }

        return (bool)value;
    }

    [return: NotNull]
    public static T NotNull<T>(
        string parameterName
        , [NotNull] T? value
    )
    {
        AssertCorrectParameterName(parameterName: parameterName);
        return value ?? throw new ArgumentNullException(paramName: parameterName, message: "Parameter cannot be null.");
    }

    private static void AssertCorrectParameterName(
        string parameterName
    )
    {
        if (parameterName is null)
        {
            throw new ArgumentNullException(paramName: nameof(parameterName), message: "Parameter cannot be null.");
        }

        if (parameterName.Length == 0)
        {
            throw new ArgumentException(message: "Parameter cannot be empty.", paramName: nameof(parameterName));
        }

        if (parameterName.Trim().Length == 0)
        {
            throw new ArgumentException(message: "Parameter cannot be whitespace.", paramName: nameof(parameterName));
        }
    }

    public static T Same<T>(
        string parameterName
        , T expected
        , T actual
    )
    {
        AssertCorrectParameterName(parameterName: parameterName);
        var expected2 = NotNull(parameterName: nameof(expected), value: expected);

        if (!expected2.Equals(obj: actual))
        {
            throw new ArgumentException(message: $"Parameter must be {expected2}. Actual: {actual}."
                , paramName: parameterName);
        }

        return actual;
    }

    public static IEnumerable<T> NotNullOrEmpty<T>(
        string parameterName
        , IEnumerable<T> value
    )
    {
        NotNull(parameterName: parameterName, value: value);

        if (!value.Any())
        {
            throw new ArgumentException(message: "Parameter cannot be empty.", paramName: parameterName);
        }

        return value;
    }

    public static T[] NotNullOrEmpty<T>(
        string parameterName
        , T[] value
    )
    {
        NotNull(parameterName: parameterName, value: value);

        if (!value.Any())
        {
            throw new ArgumentException(message: "Parameter cannot be empty.", paramName: parameterName);
        }

        return value;
    }

    public static IDictionary<TKey, TValue> NotNullOrEmpty<TKey, TValue>(
        string parameterName
        , IDictionary<TKey, TValue> value
    )
    {
        NotNull(parameterName: parameterName, value: value);
        if (!value.Any())
        {
            throw new ArgumentException(message: "Parameter cannot be empty.", paramName: parameterName);
        }

        return value;
    }

    public static string NotNullOrEmptyOrWhitespace(
        string parameterName
        , string value
        , bool trim = false
    )
    {
        NotNullOrEmpty(parameterName: parameterName, value: value);

        var trimmedValue = value.Trim();
        if (trimmedValue.Length == 0)
        {
            throw new ArgumentException(message: "Parameter cannot be whitespace.", paramName: parameterName);
        }

        if (trim)
        {
            return trimmedValue;
        }

        return value;
    }

    public static T MinimumAndNotNull<T>(
        string parameterName
        , T value
        , T smallest
    )
        where T : IComparable
    {
        NotNull(parameterName: parameterName, value: value);

        if (value.CompareTo(obj: smallest) < 0)
        {
            throw new ArgumentOutOfRangeException(paramName: parameterName, actualValue: value
                , message: $"Parameter cannot be less than {smallest}.");
        }

        return value;
    }

    public static IEnumerable<T> InRange<T>(
        string containerName
        , IEnumerable<T> container
        , int minCount
        , int maxCount
    )
    {
        var count = container.Count();
        if (count < minCount || count > maxCount)
        {
            throw new ArgumentOutOfRangeException(paramName: containerName, actualValue: count
                , message: $"{containerName}.Count() cannot be less than {minCount} or greater than {maxCount}.");
        }

        return container;
    }

    public static T InRangeAndNotNull<T>(
        string parameterName
        , T value
        , T smallest
        , T greatest
    )
        where T : IComparable
    {
        NotNull(parameterName: parameterName, value: value);

        if (value.CompareTo(obj: smallest) < 0)
        {
            throw new ArgumentOutOfRangeException(paramName: parameterName, actualValue: value
                , message: $"Parameter cannot be less than {smallest}.");
        }

        if (value.CompareTo(obj: greatest) > 0)
        {
            throw new ArgumentOutOfRangeException(paramName: parameterName, actualValue: value
                , message: $"Parameter cannot be greater than {greatest}.");
        }

        return value;
    }

    /// <summary>
    ///     Corrects the string:
    ///     If the string is null, it'll be empty.
    ///     Trims the string.
    /// </summary>
    [return: NotNull]
    public static string Correct(
        string? str
    ) =>
        string.IsNullOrWhiteSpace(value: str)
            ? ""
            : str.Trim();
}