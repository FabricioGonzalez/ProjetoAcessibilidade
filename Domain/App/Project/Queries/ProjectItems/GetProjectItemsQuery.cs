using LanguageExt.Common;
using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Queries.ProjectItems;

public sealed record GetProjectItemsQuery(
    string SolutionPath
) : IRequest<Result<IEnumerable<ExplorerItem>>>;

public sealed class GetProjectItemsQueryHandler : IHandler<GetProjectItemsQuery, Result<IEnumerable<ExplorerItem>>>
{
    private readonly IExplorerItemRepository _repository;

    public GetProjectItemsQueryHandler(
        IExplorerItemRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<ExplorerItem>>> HandleAsync(
        GetProjectItemsQuery request
        , CancellationToken cancellationToken
    ) =>
        await _repository
            .GetAllItemsAsync(request.SolutionPath);
}