using AppRepositories.Solution.Dto;

namespace XmlDatasource.Solution.Mappers;

public static class ProjectItemExtensions
{
    public static IEnumerable<ProjectItem> ToProjectItem(
        this IEnumerable<DTO.ProjectItems> projectItems
    ) =>
        projectItems
            .GroupBy(it => it.ItemName)
            .SelectMany(group =>
                group.SelectMany(LocationGroups => LocationGroups.LocationGroups
                    .GroupBy(it => it.Name)
                    .SelectMany(locationGroup => locationGroup.SelectMany(x =>
                        x.ItemsGroup.Select(it => new ProjectItem
                        {
                            Name = it.Name, Group = x.Name, Locale = LocationGroups.ItemName
                            , TemplateName = it.TemplateName
                        })))));
}