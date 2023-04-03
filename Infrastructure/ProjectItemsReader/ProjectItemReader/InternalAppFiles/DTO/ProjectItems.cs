using System.Xml.Serialization;

namespace ProjectItemReader.InternalAppFiles.DTO;

public class ProjectItems
{
    [XmlAttribute("name")]
    public string ItemName
    {
        get; set;
    }

    [XmlArray(elementName: "item_groups")]
    [XmlArrayItem(elementName: "item")]
    public List<ItemGroup> ItemGroup
    {
        get; set;
    }
}