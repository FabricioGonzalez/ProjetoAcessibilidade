using Common;

using Core.Entities.Solution.Explorer;

using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Queries.GetProjectItems;

public sealed record GetProjectItemsQuery(string SolutionPath) : IRequest<Resource<List<ExplorerItem>>>;

public sealed class GetProjectItemsQueryHandler : IQueryHandler<GetProjectItemsQuery, Resource<List<ExplorerItem>>>
{

    private readonly IExplorerItemRepository repository;

    public GetProjectItemsQueryHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Resource<List<ExplorerItem>>> Handle(GetProjectItemsQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllItemsAsync(request.SolutionPath);
        List<ExplorerItem>? res;

        result
            .OnError(out _, out var message)
            .OnLoading(out _, out _)
            .OnSuccess(out res);

        if (res is not null && res.Count > 0)
        {
            return new Resource<List<ExplorerItem>>.Success(Data: res);
        }

        return new Resource<List<ExplorerItem>>.Error(Message: message, Data: default);
    }
}
