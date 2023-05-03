using Common;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Project.Queries.ProjectItems;

public sealed record GetProjectItemContentQuery(
    string ItemPath
) : IRequest<Resource<AppItemModel>>;

public sealed class GetProjectItemContentQueryHandler
    : IHandler<GetProjectItemContentQuery, Resource<AppItemModel>>
{
    public async Task<Resource<AppItemModel>> HandleAsync(
        GetProjectItemContentQuery query
        , CancellationToken cancellation
    )
    {
        var result = await Locator.Current.GetService<IProjectItemContentRepository>()
            .GetProjectItemContent(filePathToWrite: query.ItemPath);

        if (result is not null)
        {
            result.Id = string.IsNullOrWhiteSpace(value: result.Id) ? Guid.NewGuid().ToString() : result.Id;

            return new Resource<AppItemModel>.Success(Data: result);
        }

        return new Resource<AppItemModel>.Error(Data: result
            , Message: $"Erro ao ler arquivo {Path.GetFileName(path: query.ItemPath)}");
    }
}