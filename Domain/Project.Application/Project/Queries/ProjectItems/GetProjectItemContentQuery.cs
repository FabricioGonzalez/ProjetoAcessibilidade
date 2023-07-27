using Core.Entities.Solution.Project.AppItem;
using LanguageExt.Common;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Queries.ProjectItems;

public sealed record GetProjectItemContentQuery(
    string ItemPath
) : IRequest<Result<AppItemModel>>;

public sealed class GetProjectItemContentQueryHandler
    : IHandler<GetProjectItemContentQuery, Result<AppItemModel>>
{
    private readonly IProjectItemContentRepository _repository;

    public GetProjectItemContentQueryHandler(
        IProjectItemContentRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<Result<AppItemModel>> HandleAsync(
        GetProjectItemContentQuery query
        , CancellationToken cancellation
    ) =>
        (await _repository.GetProjectItemContent(query.ItemPath))
        .Match
        (success =>
            {
                success.Id = string.IsNullOrWhiteSpace(success.Id)
                    ? Guid.NewGuid()
                        .ToString()
                    : success.Id;

                return new Result<AppItemModel>(success);
            },
            error => new Result<AppItemModel>(error));
}