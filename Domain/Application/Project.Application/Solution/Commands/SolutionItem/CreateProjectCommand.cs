using Common;
using Core.Entities.Solution;
using Project.Domain.Contracts;
using Project.Domain.Solution.Contracts;

namespace Project.Domain.Solution.Commands.SolutionItem;

public sealed record CreateSolutionCommand(
        string SolutionPath
        , ProjectSolutionModel SolutionData
    )
    : IRequest<Resource<ProjectSolutionModel>>;

public sealed class CreateSolutionCommandHandler
    : ICommandHandler<CreateSolutionCommand, Resource<ProjectSolutionModel>>
{
    public ISolutionRepository repository;

    public CreateSolutionCommandHandler(
        ISolutionRepository solutionRepository
    )
    {
        repository = solutionRepository;
    }


    public async Task<Resource<ProjectSolutionModel>> Handle(
        CreateSolutionCommand query
        , CancellationToken cancellation
    )
    {
        if (string.IsNullOrEmpty(value: query.SolutionPath))
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "Caminho da solução não pode ser vázio"
                , Data: default);
        }

        await repository.SaveSolution(solutionPath: query.SolutionPath, dataToWrite: query.SolutionData);

        /*if (result is null)
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "A solução não foi encontrada", Data: default);
        }*/
        return new Resource<ProjectSolutionModel>.Success(Data: query.SolutionData);
    }
}