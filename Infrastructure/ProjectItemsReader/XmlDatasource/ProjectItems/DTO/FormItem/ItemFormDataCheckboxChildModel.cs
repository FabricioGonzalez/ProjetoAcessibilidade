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

    [XmlArray("options")]
    [XmlArrayItem("option")]
    public List<ItemOptionModel> Options
    {
        get;
        set;
    }

    [XmlArray("texts")]
    [XmlArrayItem("text_item")]
    public List<ItemFormDataTextModel> TextItems
    {
        get;
        set;
    }
}