using Common;

using Core.Entities.Solution.Explorer;

using MediatR;

using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Queries.GetProjectItems;

public record GetProjectItemsQuery : IRequest<List<ExplorerItem>>
{
    public GetProjectItemsQuery(string solutionPath)
    {
        this.solutionPath = solutionPath;
    }
    public string solutionPath
    {
        get; init;
    }

}
public class GetProjectItemsQueryHandler : IQueryHandler<GetProjectItemsQuery, Resource<List<ExplorerItem>>>
{

    private readonly IExplorerItemRepository repository;

    public GetProjectItemsQueryHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Resource<List<ExplorerItem>>> Handle(GetProjectItemsQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllItemsAsync(request.solutionPath);

        result
            .OnError(out var res, out var message)
            .OnLoading(out res, out var isLoading)
            .OnSuccess(out res);

        if (res is not null && res.Count > 0)
        {
            return new Resource<List<ExplorerItem>>.Success(Data: res);
        }

        return new Resource<List<ExplorerItem>>.Error(Message: "", Data: default);
    }
}
