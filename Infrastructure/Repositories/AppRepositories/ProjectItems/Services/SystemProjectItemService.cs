using AppRepositories.ProjectItems.Contracts;

namespace AppRepositories.ProjectItems.Services;

public class SystemProjectItemService
{
    private readonly IProjectItemDatasource _datasource;

    public SystemProjectItemService(
        IProjectItemDatasource datasource
    )
    {
        _datasource = datasource;
    }
}