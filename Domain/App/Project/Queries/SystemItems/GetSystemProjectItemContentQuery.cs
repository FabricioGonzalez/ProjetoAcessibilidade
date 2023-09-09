using Core.Entities.Solution.Project.AppItem;
using LanguageExt.Common;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Queries.SystemItems;

public sealed record GetSystemProjectItemContentQuery(
    string ItemPath
) : IRequest<Result<AppItemModel>>;

public sealed class GetSystemProjectItemContentQueryHandler
    : IHandler<GetSystemProjectItemContentQuery, Result<AppItemModel>>
{
    private readonly IProjectItemContentRepository _repository;

    public GetSystemProjectItemContentQueryHandler(
        IProjectItemContentRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<Result<AppItemModel>> HandleAsync(
        GetSystemProjectItemContentQuery query
        , CancellationToken cancellation
    ) =>
        await _repository.GetSystemProjectItemContentSerealizer(query.ItemPath);
}