using System.Collections.Generic;
using Newtonsoft.Json;
using ProjectAvalonia.Common.Json.Converters;

namespace ProjectAvalonia.Common.Json;

public class JsonSerializationOptions
{
    private static readonly JsonSerializerSettings CurrentSettings = new()
    {
        Converters = new List<JsonConverter>
        {
            new TimeSpanJsonConverter()
        }
    };

    public static readonly JsonSerializationOptions Default = new();

    private JsonSerializationOptions()
    {
    }

    public JsonSerializerSettings Settings => CurrentSettings;
}