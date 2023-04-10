using System.Xml.Serialization;

namespace ProjectItemReader.InternalAppFiles.DTO;

public class ItemGroup
{
    [XmlElement(elementName: "id")]
    public string Id
    {
        get;
        set;
    }

    [XmlElement(elementName: "name")]
    public string Name
    {
        get;
        set;
    }

    [XmlElement(elementName: "item_path")]
    public string ItemPath
    {
        get;
        set;
    }

    [XmlElement(elementName: "template_name")]
    public string TemplateName
    {
        get;
        set;
    }
}