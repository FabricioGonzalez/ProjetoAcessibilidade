using Common;

using Core.Entities.Solution;

using MediatR;

using Project.Domain.Contracts;
using Project.Domain.Solution.Contracts;

namespace Project.Domain.Solution.Queries;
public class ReadSolutionProjectQuery : IRequest<Resource<ProjectSolutionModel>>
{
    public ReadSolutionProjectQuery(string solutionPath)
    {
        SolutionPath = solutionPath;
    }
    public string SolutionPath
    {
        get;
    }
}

public class ReadSolutionProjectQueryHandler : IQueryHandler<ReadSolutionProjectQuery, Resource<ProjectSolutionModel>>
{
    public ISolutionRepository repository;
    public ReadSolutionProjectQueryHandler(ISolutionRepository solutionRepository)
    {
        repository = solutionRepository;
    }


    public async Task<Resource<ProjectSolutionModel>> Handle(ReadSolutionProjectQuery query, CancellationToken cancellation)
    {
        if (string.IsNullOrEmpty(query.SolutionPath))
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "Caminho da solução não pode ser vázio", Data: default);
        }
        var result = await repository.ReadSolution(query.SolutionPath);

        if (result is null)
        {
            return new Resource<ProjectSolutionModel>.Error(Message: "A solução não foi encontrada", Data: default);
        }
        return new Resource<ProjectSolutionModel>.Success(Data: result);
    }
}
