using App.Core.Entities.Solution.Explorer;

using Common;

using MediatR;

using Project.Application.Project.Contracts;

namespace Project.Application.Project.Queries.GetProjectItems;

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
public class GetProjectItemsQueryHandler
{

    private readonly IExplorerItemRepository repository;

    public GetProjectItemsQueryHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }
    public async Task<List<ExplorerItem>> Handle(GetProjectItemsQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllItemsAsync(request.solutionPath);

        result
            .OnError(out var res, out var message)
            .OnLoading(out res, out var isLoading)
            .OnSuccess(out res);

        if(res is not null && res.Count > 0)
        {
            return res;
        }

        return new();
    }
}
