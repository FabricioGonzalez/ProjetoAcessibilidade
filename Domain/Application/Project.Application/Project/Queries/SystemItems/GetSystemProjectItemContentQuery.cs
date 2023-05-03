using Common;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Project.Queries.SystemItems;

public sealed record GetSystemProjectItemContentQuery(
    string ItemPath
) : IRequest<Resource<AppItemModel>>;

public sealed class GetSystemProjectItemContentQueryHandler
    : IHandler<GetSystemProjectItemContentQuery, Resource<AppItemModel>>
{
    public async Task<Resource<AppItemModel>> HandleAsync(
        GetSystemProjectItemContentQuery query
        , CancellationToken cancellation
    )
    {
        var result = await Locator.Current.GetService<IProjectItemContentRepository>()
            .GetSystemProjectItemContentSerealizer(filePathToWrite: query.ItemPath);

        if (result is not null)
        {
            return new Resource<AppItemModel>.Success(Data: result);
        }

        return new Resource<AppItemModel>.Error(Data: result
            , Message: $"Erro ao ler arquivo {Path.GetFileName(path: query.ItemPath)}");
    }
}