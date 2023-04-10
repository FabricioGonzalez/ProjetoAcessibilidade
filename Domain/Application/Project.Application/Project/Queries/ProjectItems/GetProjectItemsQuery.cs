using Common;
using Core.Entities.Solution.Explorer;
using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Queries.ProjectItems;

public sealed record GetProjectItemsQuery(
    string SolutionPath
) : IRequest<Resource<List<ExplorerItem>>>;

public sealed class GetProjectItemsQueryHandler : IQueryHandler<GetProjectItemsQuery, Resource<List<ExplorerItem>>>
{
    private readonly IExplorerItemRepository repository;

    public GetProjectItemsQueryHandler(
        IExplorerItemRepository repository
    )
    {
        this.repository = repository;
    }

    public async Task<Resource<List<ExplorerItem>>> Handle(
        GetProjectItemsQuery request
        , CancellationToken cancellationToken
    )
    {
        var result = await repository.GetAllItemsAsync(solutionPath: request.SolutionPath);

        return result switch
        {
            Resource<List<ExplorerItem>>.Success success =>
                new Resource<List<ExplorerItem>>.Success(Data: success.Data)
            , Resource<List<ExplorerItem>>.Error error => new Resource<List<ExplorerItem>>.Error(
                Message: error.Message, Data: error.Data)
            , _ =>
                new Resource<List<ExplorerItem>>.Error(Message: "No data was found", Data: null)
        };
    }
}