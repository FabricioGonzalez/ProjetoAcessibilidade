using System.ComponentModel;

namespace ProjectAvalonia.Common.Json.Converters.Timing;

public class DefaultValueTimeSpanAttribute : DefaultValueAttribute
{
    public DefaultValueTimeSpanAttribute(
        string json
    ) : base(value: TimeSpanJsonConverter.Parse(stringValue: json))
    {
    }
}