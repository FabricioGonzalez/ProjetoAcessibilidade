using Common.Result;

using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Queries.ProjectItems;

public sealed record GetProjectItemsQuery(
    string SolutionPath
) : IRequest<Result<IEnumerable<ExplorerItem>, Exception>>;

public sealed class GetProjectItemsQueryHandler : IHandler<GetProjectItemsQuery, Result<IEnumerable<ExplorerItem>, Exception>>
{
    private readonly IExplorerItemRepository _repository;

    public GetProjectItemsQueryHandler(
        IExplorerItemRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<ExplorerItem>, Exception>> HandleAsync(
        GetProjectItemsQuery request,
        CancellationToken cancellationToken
    )
    {
        Result<IEnumerable<ExplorerItem>, Exception> result = await _repository
            .GetAllItemsAsync(solutionPath: request.SolutionPath);

        return result;
    }
}