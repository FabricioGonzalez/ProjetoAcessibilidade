using Common;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Solution.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Solution.Queries;

public sealed record ReadSolutionProjectQuery(
    string SolutionPath
) : IRequest<Resource<ProjectSolutionModel>>;

public sealed class ReadSolutionProjectQueryHandler
    : IHandler<ReadSolutionProjectQuery, Resource<ProjectSolutionModel>>
{
    /*public ISolutionRepository repository;

    public ReadSolutionProjectQueryHandler(
        ISolutionRepository solutionRepository
    )
    {
        repository = solutionRepository;
    }*/


    public async Task<Resource<ProjectSolutionModel>> HandleAsync(
        ReadSolutionProjectQuery query
        , CancellationToken cancellation
    )
    {
        if (string.IsNullOrEmpty(value: query.SolutionPath))
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "Caminho da solução não pode ser vázio"
                , Data: default);
        }

        var result = await Locator.Current.GetService<ISolutionRepository>()
            ?.ReadSolution(solutionPath: query.SolutionPath);

        if (result is null)
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "A solução não foi encontrada", Data: default);
        }

        return new Resource<ProjectSolutionModel>.Success(Data: result);
    }
}