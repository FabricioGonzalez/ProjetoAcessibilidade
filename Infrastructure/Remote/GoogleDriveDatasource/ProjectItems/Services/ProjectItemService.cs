using AppRepositories.ProjectItems.Contracts;

namespace GoogleDriveDatasource.ProjectItems.Services;

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