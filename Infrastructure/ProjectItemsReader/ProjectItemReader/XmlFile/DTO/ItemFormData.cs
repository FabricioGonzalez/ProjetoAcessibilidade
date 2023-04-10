using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.DTO;

public abstract class ItemFormData
{
    public ItemFormData()
    {
    }

    public ItemFormData(
        string id
    )
    {
        Id = id;
    }

    [XmlElement(elementName: "id")]
    public string Id
    {
        get;
        set;
    }
}