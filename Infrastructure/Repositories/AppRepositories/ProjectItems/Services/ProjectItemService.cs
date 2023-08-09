using AppRepositories.ProjectItems.Contracts;

namespace AppRepositories.ProjectItems.Services;

public class ProjectItemService
{
    private readonly IProjectItemDatasource _datasource;

    public ProjectItemService(
        IProjectItemDatasource datasource
    )
    {
        _datasource = datasource;
    }
}