using System.Collections.Generic;
using System.Text.Json.Serialization;

using Core.Contracts;

namespace Core.Models;

public class ItemModel
{
    [JsonPropertyName("item")]
    public string ItemName { get; set; }
    [JsonPropertyName("tabela")]
    public List<FormDataItemModel> FormData { get; set; }
    [JsonPropertyName("Lei")]
    public IEnumerable<LawModel> LawList { get; set; }
}
