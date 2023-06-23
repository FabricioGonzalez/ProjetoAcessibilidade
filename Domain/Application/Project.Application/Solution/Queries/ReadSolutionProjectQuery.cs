using Common.Optional;
using Common.Result;

using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Solution.Contracts;

namespace ProjetoAcessibilidade.Domain.Solution.Queries;

public sealed record ReadSolutionProjectQuery(
    string SolutionPath
) : IRequest<Result<ProjectSolutionModel, Exception>>;

public sealed class ReadSolutionProjectQueryHandler
    : IHandler<ReadSolutionProjectQuery, Result<ProjectSolutionModel, Exception>>
{
    public ISolutionRepository _repository;

    public ReadSolutionProjectQueryHandler(
        ISolutionRepository solutionRepository
    )
    {
        _repository = solutionRepository;
    }


    public async Task<Result<ProjectSolutionModel, Exception>> HandleAsync(
        ReadSolutionProjectQuery query,
        CancellationToken cancellation
    ) =>
        await _repository.ReadSolution(query.SolutionPath.ToOption());
}