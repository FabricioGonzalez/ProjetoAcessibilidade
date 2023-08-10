using LanguageExt;
using LanguageExt.Common;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Solution.Contracts;

namespace ProjetoAcessibilidade.Domain.Solution.Queries;

public sealed record ReadSolutionProjectQuery(
    string SolutionPath
) : IRequest<Result<ProjectSolutionModel>>;

public sealed class ReadSolutionProjectQueryHandler
    : IHandler<ReadSolutionProjectQuery, Result<ProjectSolutionModel>>
{
    public ISolutionRepository _repository;

    public ReadSolutionProjectQueryHandler(
        ISolutionRepository solutionRepository
    )
    {
        _repository = solutionRepository;
    }


    public async Task<Result<ProjectSolutionModel>> HandleAsync(
        ReadSolutionProjectQuery query
        , CancellationToken cancellation
    ) =>
        await _repository.ReadSolution(Option<string>.Some(query.SolutionPath));
}