using AppRepositories.Solution.Dto;
using XmlDatasource.Solution.DTO;

namespace XmlDatasource.Solution.Mappers;

public static class XmlSolutionExtensions
{
    public static SolutionItem ToSolutionItem(
        this SolutionItemRoot root
    ) =>
        new()
        {
            Report = root.Report.ToReportItem()
            , ProjectItems = root.ProjectItems.ToProjectItem().ToList()
        };

    public static SolutionItemRoot ToSolutionItemRoot(
        this SolutionItem solutionItem
    ) =>
        new();
}