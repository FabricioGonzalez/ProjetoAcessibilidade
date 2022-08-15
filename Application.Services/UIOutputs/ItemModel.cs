using System.Collections.Generic;
using System.Text.Json.Serialization;

using Core.Contracts;

namespace SystemApplication.Services.UIOutputs;

public class ItemModel
{
    [JsonPropertyName("item")]
    public string ItemName
    {
        get; set;
    }
    [JsonPropertyName("tabela")]
    public List<IFormDataItemContract> FormData
    {
        get; set;
    }
    [JsonPropertyName("Lei")]
    public List<LawModel> LawList
    {
        get; set;
    }
}
