using Common;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Solution.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Solution.Commands.SolutionItem;

public sealed record SyncSolutionCommand(
    string SolutionPath
    , ProjectSolutionModel SolutionData
) : IRequest<Resource<ProjectSolutionModel>>;

public sealed class SyncSolutionCommandHandler : IHandler<SyncSolutionCommand, Resource<ProjectSolutionModel>>
{
    public async Task<Resource<ProjectSolutionModel>> HandleAsync(
        SyncSolutionCommand query
        , CancellationToken cancellation
    )
    {
        if (string.IsNullOrEmpty(value: query.SolutionPath))
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "Caminho da solução não pode ser vázio"
                , Data: default);
        }

        await Locator.Current.GetService<ISolutionRepository>()
            .SyncSolution(solutionPath: query.SolutionPath, dataToWrite: query.SolutionData);

        /*if (result is null)
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "A solução não foi encontrada", Data: default);
        }*/
        return new Resource<ProjectSolutionModel>.Success(Data: query.SolutionData);
    }
}