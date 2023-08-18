using System.Xml.Serialization;

namespace XmlDatasource.Solution.DTO;

[XmlRoot("solution")]
public sealed class SolutionItemRoot
{
    [XmlIgnore]
    public string SolutionPath
    {
        get;
        set;
    }

    [XmlElement("report")]
    public ReportItem Report
    {
        get;
        set;
    }

    [XmlArray("project_items")]
    [XmlArrayItem("project_item")]
    public List<ProjectItems> ProjectItems
    {
        get;
        set;
    }
}