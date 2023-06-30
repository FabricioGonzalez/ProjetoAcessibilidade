using Common.Result;

using Core.Entities.Solution.Project.AppItem;

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
        GetProjectItemContentQuery query,
        CancellationToken cancellation
    )
    {
        return (await _repository.GetProjectItemContent(filePathToWrite: query.ItemPath))
            .Match
            (success =>
            {
                success.Id = string.IsNullOrWhiteSpace(value: success.Id)
               ? Guid.NewGuid()
                   .ToString()
               : success.Id;

                return Result<AppItemModel>.Success(success);
            },
        Result<AppItemModel>.Failure);
        /*
                if (result is not null)
                {


                    return new Resource<AppItemModel>.Success(Data: result);
                }

                return new Resource<AppItemModel>.Error(
                    Data: result,
                    Message: $"Erro ao ler arquivo {Path.GetFileName(path: query.ItemPath)}");*/
    }
}