using System.Xml.Serialization;

namespace XmlDatasource.ProjectItems.DTO.FormItem;

public class ItemFormDataCheckboxChildModel : ItemFormData
{
    public ItemFormDataCheckboxChildModel()
    {
    }

    public ItemFormDataCheckboxChildModel(
        string id
        , string topic
        , bool isInvalid
    )
        : base(id)
    {
        Id = id;
        Topic = topic;
        IsInvalid = isInvalid;
    }

    [XmlElement("is_valid", IsNullable = true)]
    public bool? IsInvalid
    {
        get;
        set;
    }

    [XmlElement("topic", IsNullable = true)]
    public string Topic
    {
        get;
        set;
    }

    [XmlArray("options", IsNullable = true)]
    [XmlArrayItem("option", IsNullable = true)]
    public List<ItemOptionModel>? Options
    {
        get;
        set;
    }

    [XmlArray("texts", IsNullable = true)]
    [XmlArrayItem("text_item", IsNullable = true)]
    public List<ItemFormDataTextModel>? TextItems
    {
        get;
        set;
    }
}