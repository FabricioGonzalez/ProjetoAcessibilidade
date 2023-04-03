using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.DTO;
public abstract class ItemFormDataContainer
{
    public ItemFormDataContainer()
    {

    }
    public ItemFormDataContainer(string id, string topic, ItemFormDataEnum type)
    {
        Id = id;
        Topic = topic;
        Type = type;
    }
    [XmlElement(elementName: "id")]
    public string Id
    {
        get; set;
    }
    [XmlElement(elementName: "topic", IsNullable = true)]
    public string Topic
    {
        get; set;
    }
    [XmlElement(elementName: "type")]
    public ItemFormDataEnum Type
    {
        get; set;
    }
}
