using Common;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Solution.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Solution.Commands.SolutionItem;

public sealed record CreateSolutionCommand(
        string SolutionPath
        , ProjectSolutionModel SolutionData
    )
    : IRequest<Resource<ProjectSolutionModel>>;

public sealed class CreateSolutionCommandHandler
    : IHandler<CreateSolutionCommand, Resource<ProjectSolutionModel>>
{
    public async Task<Resource<ProjectSolutionModel>> HandleAsync(
        CreateSolutionCommand query
        , CancellationToken cancellation
    )
    {
        if (string.IsNullOrEmpty(value: query.SolutionPath))
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "Caminho da solução não pode ser vázio"
                , Data: default);
        }

        await Locator.Current.GetService<ISolutionRepository>()
            .SaveSolution(solutionPath: query.SolutionPath, dataToWrite: query.SolutionData);

        /*if (result is null)
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "A solução não foi encontrada", Data: default);
        }*/
        return new Resource<ProjectSolutionModel>.Success(Data: query.SolutionData);
    }
}