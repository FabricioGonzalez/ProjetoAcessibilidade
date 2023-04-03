using System.Xml.Serialization;

using Core.Entities.Solution;

namespace ProjectItemReader.InternalAppFiles.DTO;
[XmlRoot(elementName: "solution")]
public class SolutionItemRoot
{
    [XmlElement(elementName: "report")]
    public ReportItem Report
    {
        get; set;
    }
    [XmlArray("project_items")]
    [XmlArrayItem("item_groups")]
    public List<ProjectItems> ProjectItems
    {
        get; set;
    }
}

public static partial class Extensions
{
    public static SolutionItemRoot ToItemRoot(this ProjectSolutionModel model)
    {
        return new SolutionItemRoot()
        {
            Report = new ReportItem
            {
                /*Data = model.SolutionReportInfo*/
            },
            ProjectItems = model.ItemGroups.Select(item => new ProjectItems()
            {
                ItemName = item.Name,
                ItemGroup = item.Items.Select(i => new ItemGroup()
                {
                    Id = i.Id,
                    ItemPath = i.ItemPath,
                    Name = i.Name,
                    TemplateName = i.TemplateName
                }).ToList()
            }).ToList()
        };
    }
    public static ProjectSolutionModel ToSolutionInfo(this SolutionItemRoot model)
    {
        return new ProjectSolutionModel()
        {
        };
    }
}
