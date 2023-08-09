using Application.Solution.Contracts;
using AppRepositories.Solution.Contracts;
using Domain.Solutions;

namespace AppRepositories.Solution.Services;

public class SolutionService : ISolutionRepository
{
    private readonly ISolutionDatasource _datasource;

    public SolutionService(
        ISolutionDatasource datasource
    )
    {
        _datasource = datasource;
    }

    public Task<ISolution> OpenSolution(
        string path
    ) =>
        throw new NotImplementedException();

    public Task SaveSolution(
        string path
        , ISolution solutionToSave
    ) =>
        throw new NotImplementedException();
}