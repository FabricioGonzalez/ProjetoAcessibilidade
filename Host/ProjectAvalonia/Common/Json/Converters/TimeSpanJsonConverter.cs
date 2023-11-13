using System;
using Newtonsoft.Json;

namespace ProjectAvalonia.Common.Json.Converters;

public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
{
    /// <inheritdoc />
    public override TimeSpan ReadJson(
        JsonReader reader
        , Type objectType
        , TimeSpan existingValue
        , bool hasExistingValue
        , JsonSerializer serializer
    )
    {
        var stringValue = reader.Value as string;
        return Parse(stringValue: stringValue);
    }

    public static TimeSpan Parse(
        string? stringValue
    )
    {
        if (string.IsNullOrWhiteSpace(value: stringValue))
        {
            return default;
        }

        var daysParts = stringValue.Split(separator: 'd');
        var days = int.Parse(s: daysParts[0].Trim());
        var hoursParts = daysParts[1].Split(separator: 'h');
        var hours = int.Parse(s: hoursParts[0].Trim());
        var minutesParts = hoursParts[1].Split(separator: 'm');
        var minutes = int.Parse(s: minutesParts[0].Trim());
        var secondsParts = minutesParts[1].Split(separator: 's');
        var seconds = int.Parse(s: secondsParts[0].Trim());
        return new TimeSpan(days: days, hours: hours, minutes: minutes, seconds: seconds);
    }

    /// <inheritdoc />
    public override void WriteJson(
        JsonWriter writer
        , TimeSpan value
        , JsonSerializer serializer
    )
    {
        var ts = value;
        writer.WriteValue(value: $"{ts.Days}d {ts.Hours}h {ts.Minutes}m {ts.Seconds}s");
    }
}