using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.DTO.FormItem;

public class ItemFormDataCheckboxChildModel : ItemFormData
{
    public ItemFormDataCheckboxChildModel()
    {
    }

    public ItemFormDataCheckboxChildModel(
        string id
        , string topic
    )
        : base(id: id)
    {
        Id = id;
        Topic = topic;
    }

    [XmlElement(elementName: "topic", IsNullable = true)]
    public string Topic
    {
        get;
        set;
    }

    [XmlArray(elementName: "options")]
    [XmlArrayItem(elementName: "option")]
    public List<ItemOptionModel> Options
    {
        get;
        set;
    }

    [XmlArray(elementName: "texts")]
    [XmlArrayItem(elementName: "text_item")]
    public List<ItemFormDataTextModel> TextItems
    {
        get;
        set;
    }
}