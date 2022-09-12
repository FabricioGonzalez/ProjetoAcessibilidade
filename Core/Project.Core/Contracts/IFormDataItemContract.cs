using System.Text.Json.Serialization;

using Core.Enums;

namespace Core.Contracts;

public interface IFormDataItemContract
{
    [JsonPropertyName("topico")]
    public string Topic { get; set; }
    [JsonPropertyName("tipo")]
    public FormDataItemTypeEnum Type { get; set; }
}
