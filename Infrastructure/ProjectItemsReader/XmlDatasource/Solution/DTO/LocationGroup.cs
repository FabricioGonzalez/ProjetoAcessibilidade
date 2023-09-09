using System.Xml.Serialization;

namespace XmlDatasource.Solution.DTO;

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