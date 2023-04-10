using Common;
using Core.Entities.Solution;
using Project.Domain.Contracts;
using Project.Domain.Solution.Contracts;

namespace Project.Domain.Solution.Commands.SolutionItem;

public sealed record SyncSolutionCommand(
    string SolutionPath
    , ProjectSolutionModel SolutionData
) : IRequest<Resource<ProjectSolutionModel>>;

public sealed class SyncSolutionCommandHandler : ICommandHandler<SyncSolutionCommand, Resource<ProjectSolutionModel>>
{
    public ISolutionRepository repository;

    public SyncSolutionCommandHandler(
        ISolutionRepository solutionRepository
    )
    {
        repository = solutionRepository;
    }


    public async Task<Resource<ProjectSolutionModel>> Handle(
        SyncSolutionCommand query
        , CancellationToken cancellation
    )
    {
        if (string.IsNullOrEmpty(value: query.SolutionPath))
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "Caminho da solução não pode ser vázio"
                , Data: default);
        }

        await repository.SyncSolution(solutionPath: query.SolutionPath, dataToWrite: query.SolutionData);

        /*if (result is null)
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "A solução não foi encontrada", Data: default);
        }*/
        return new Resource<ProjectSolutionModel>.Success(Data: query.SolutionData);
    }
}