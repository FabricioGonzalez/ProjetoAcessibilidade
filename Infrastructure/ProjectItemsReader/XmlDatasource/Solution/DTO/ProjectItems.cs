using System.Xml.Serialization;

namespace XmlDatasource.Solution.DTO;

public class ProjectItems
{
    [XmlAttribute("name")]
    public string ItemName
    {
        get;
        set;
    }

    [XmlArray("location_items")]
    [XmlArrayItem("location_item")]
    public List<LocationGroup> LocationGroups
    {
        get;
        set;
    }
}