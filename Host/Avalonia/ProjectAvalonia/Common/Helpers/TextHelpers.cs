using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectAvalonia.Common.Helpers;

public static class TextHelpers
{
    public static string AddSIfPlural(
        int n
    ) => n > 1 ? "s" : "";

    public static string CloseSentenceIfZero(
        params int[] counts
    ) => counts.All(predicate: x => x == 0) ? "." : " ";

    private static string ConcatNumberAndUnit(
        int n
        , string unit
    ) => n > 0 ? $"{n} {unit}{AddSIfPlural(n: n)}" : "";

    private static void AddIfNotEmpty(
        List<string> list
        , string item
    )
    {
        if (!string.IsNullOrEmpty(value: item))
        {
            list.Add(item: item);
        }
    }

    public static string TimeSpanToFriendlyString(
        TimeSpan time
    )
    {
        var textMembers = new List<string>();
        var result = "";

        AddIfNotEmpty(list: textMembers, item: ConcatNumberAndUnit(n: time.Days, unit: "day"));
        AddIfNotEmpty(list: textMembers, item: ConcatNumberAndUnit(n: time.Hours, unit: "hour"));
        AddIfNotEmpty(list: textMembers, item: ConcatNumberAndUnit(n: time.Minutes, unit: "minute"));
        AddIfNotEmpty(list: textMembers, item: ConcatNumberAndUnit(n: time.Seconds, unit: "second"));

        for (var i = 0; i < textMembers.Count; i++)
        {
            result += textMembers[index: i];

            if (textMembers.Count > 1 && i < textMembers.Count - 2)
            {
                result += ", ";
            }
            else if (textMembers.Count > 1 && i == textMembers.Count - 2)
            {
                result += " and ";
            }
        }

        return result;
    }

    public static string ToFormattedString(
        this string money
    )
    {
        const int WholeGroupSize = 3;

        var moneyString = money;

        moneyString = moneyString.Insert(startIndex: moneyString.Length - 4, value: " ");

        var startIndex = moneyString.IndexOf(value: ".", comparisonType: StringComparison.Ordinal) - WholeGroupSize;

        if (startIndex > 0)
        {
            for (var i = startIndex; i > 0; i -= WholeGroupSize)
            {
                moneyString = moneyString.Insert(startIndex: i, value: " ");
            }
        }

        return moneyString;
    }

    public static string ParseLabel(
        this string text
    ) => Regex.Replace(input: text, pattern: @"\s+", replacement: " ").Trim();

    public static string GetPrivacyMask(
        int repeatCount
    ) => new(c: '#', count: repeatCount);
}