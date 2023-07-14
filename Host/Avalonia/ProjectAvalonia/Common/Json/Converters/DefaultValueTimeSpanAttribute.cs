using System.ComponentModel;

namespace ProjectAvalonia.Common.Json.Converters;

public class DefaultValueTimeSpanAttribute : DefaultValueAttribute
{
    public DefaultValueTimeSpanAttribute(
        string json
    ) : base(TimeSpanJsonConverter.Parse(json))
    {
    }
}