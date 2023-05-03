using System.Xml.Serialization;
using ProjetoAcessibilidade.Core.Entities.Solution;

namespace ProjectItemReader.InternalAppFiles.DTO;

[XmlRoot(elementName: "solution")]
public class SolutionItemRoot
{
    [XmlElement(elementName: "report")]
    public ReportItem Report
    {
        get;
        set;
    }

    [XmlArray(elementName: "project_items")]
    [XmlArrayItem(elementName: "item_groups")]
    public List<ProjectItems> ProjectItems
    {
        get;
        set;
    }
}

public static class Extensions
{
    public static SolutionItemRoot ToItemRoot(
        this ProjectSolutionModel model
    ) =>
        new()
        {
            Report = new ReportItem
            {
                /*Data = model.SolutionReportInfo*/
            },
            ProjectItems = model.ItemGroups.Select(selector: item => new ProjectItems
            {
                ItemName = item.Name, ItemGroup = item.Items.Select(selector: i => new ItemGroup
                {
                    Id = i.Id, ItemPath = i.ItemPath, Name = i.Name, TemplateName = i.TemplateName
                }).ToList()
            }).ToList()
        };

    public static ProjectSolutionModel ToSolutionInfo(
        this SolutionItemRoot model
    ) => new();
}