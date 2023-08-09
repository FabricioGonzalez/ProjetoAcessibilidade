using AppRepositories.Solution.Contracts;

namespace GoogleDriveDatasource.Solution.Services;

public class SolutionService
{
    private readonly ISolutionDatasource _datasource;

    public SolutionService(
        ISolutionDatasource datasource
    )
    {
        _datasource = datasource;
    }
}