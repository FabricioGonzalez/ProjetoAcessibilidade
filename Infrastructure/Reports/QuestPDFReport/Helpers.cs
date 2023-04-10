using System.Collections.Concurrent;
using QuestPDFReport.Models;
using SkiaSharp;

namespace QuestPDFReport;

public static class Helpers
{
    private static readonly ConcurrentDictionary<int, string> RomanNumeralCache = new();

    public static Random Random
    {
        get;
    } = new(Seed: 1);

    public static string GetTestItem(
        string path
    ) => Path.Combine(path1: AppDomain.CurrentDomain.BaseDirectory, path2: "Resources", path3: path);

    public static byte[] GetImage(
        string name
    )
    {
        var photoPath = GetTestItem(path: name);
        return SKImage.FromEncodedData(filename: photoPath).EncodedData.ToArray();
    }

    public static Location RandomLocation() =>
        new()
        {
            Longitude = Random.NextDouble() * 360f - 180f, Latitude = Random.NextDouble() * 180f - 90f
        };

    public static string FormatAsRomanNumeral(
        this int number
    )
    {
        if (number < 0 || number > 3999)
        {
            throw new ArgumentOutOfRangeException(paramName: "Number should be in range from 1 to 3999");
        }

        return RomanNumeralCache.GetOrAdd(key: number, valueFactory: x =>
        {
            if (x >= 1000)
            {
                return "M" + (x - 1000).FormatAsRomanNumeral();
            }

            if (x >= 900)
            {
                return "CM" + (x - 900).FormatAsRomanNumeral();
            }

            if (x >= 500)
            {
                return "D" + (x - 500).FormatAsRomanNumeral();
            }

            if (x >= 400)
            {
                return "CD" + (x - 400).FormatAsRomanNumeral();
            }

            if (x >= 100)
            {
                return "C" + (x - 100).FormatAsRomanNumeral();
            }

            if (x >= 90)
            {
                return "XC" + (x - 90).FormatAsRomanNumeral();
            }

            if (x >= 50)
            {
                return "L" + (x - 50).FormatAsRomanNumeral();
            }

            if (x >= 40)
            {
                return "XL" + (x - 40).FormatAsRomanNumeral();
            }

            if (x >= 10)
            {
                return "X" + (x - 10).FormatAsRomanNumeral();
            }

            if (x >= 9)
            {
                return "IX" + (x - 9).FormatAsRomanNumeral();
            }

            if (x >= 5)
            {
                return "V" + (x - 5).FormatAsRomanNumeral();
            }

            if (x >= 4)
            {
                return "IV" + (x - 4).FormatAsRomanNumeral();
            }

            if (x >= 1)
            {
                return "I" + (x - 1).FormatAsRomanNumeral();
            }

            return string.Empty;
        });
    }
}