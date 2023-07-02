using Common;
using LanguageExt;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Solution.Contracts;

namespace ProjetoAcessibilidade.Domain.Solution.Commands.SolutionItem;

public sealed record SyncSolutionCommand(
    string SolutionPath
    , ProjectSolutionModel SolutionData
) : IRequest<Resource<ProjectSolutionModel>>;

public sealed class SyncSolutionCommandHandler : IHandler<SyncSolutionCommand, Resource<ProjectSolutionModel>>
{
    private readonly ISolutionRepository _repository;

    public SyncSolutionCommandHandler(
        ISolutionRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<Resource<ProjectSolutionModel>> HandleAsync(
        SyncSolutionCommand query
        , CancellationToken cancellation
    )
    {
        if (string.IsNullOrEmpty(query.SolutionPath))
        {
            return new Resource<ProjectSolutionModel>.Error(
                "Caminho da solução não pode ser vázio");
        }

        await _repository
            .SyncSolution(
                Option<string>.Some(query.SolutionPath),
                Option<ProjectSolutionModel>.Some(query.SolutionData));

        /*if (result is null)
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "A solução não foi encontrada", Data: default);
        }*/
        return new Resource<ProjectSolutionModel>.Success(query.SolutionData);
    }
}