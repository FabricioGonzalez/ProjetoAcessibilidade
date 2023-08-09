using System.Xml.Serialization;

namespace XmlDatasource.InternalAppFiles.DTO;

public class ItemGroup
{
    [XmlElement("id")]
    public string Id
    {
        get;
        set;
    }

    [XmlElement("name")]
    public string Name
    {
        get;
        set;
    }

    [XmlElement("item_path")]
    public string ItemPath
    {
        get;
        set;
    }

    [XmlElement("template_name")]
    public string TemplateName
    {
        get;
        set;
    }
}

public class LocationGroup
{
    [XmlAttribute("name")]
    public string Name
    {
        get;
        set;
    }

    [XmlArray("items")]
    [XmlArrayItem("item")]
    public List<ItemGroup> ItemsGroup
    {
        get;
        set;
    }
}