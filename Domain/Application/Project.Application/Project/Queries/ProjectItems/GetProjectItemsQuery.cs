using Common;
using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Project.Queries.ProjectItems;

public sealed record GetProjectItemsQuery(
    string SolutionPath
) : IRequest<Resource<List<ExplorerItem>>>;

public sealed class GetProjectItemsQueryHandler : IHandler<GetProjectItemsQuery, Resource<List<ExplorerItem>>>
{
    public async Task<Resource<List<ExplorerItem>>> HandleAsync(
        GetProjectItemsQuery request
        , CancellationToken cancellationToken
    )
    {
        var result = await Locator.Current.GetService<IExplorerItemRepository>()
            .GetAllItemsAsync(solutionPath: request.SolutionPath);

        return result switch
        {
            Resource<List<ExplorerItem>>.Success success =>
                new Resource<List<ExplorerItem>>.Success(Data: success.Data),
            Resource<List<ExplorerItem>>.Error error => new Resource<List<ExplorerItem>>.Error(
                Message: error.Message, Data: error.Data),
            _ =>
                new Resource<List<ExplorerItem>>.Error(Message: "No data was found", Data: null)
        };
    }
}