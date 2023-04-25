using Common;
using Core.Entities.Solution.Project.AppItem;
using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Queries.ProjectItems;

public sealed record GetProjectItemContentQuery(
    string ItemPath
) : IRequest<Resource<AppItemModel>>;

public sealed class GetProjectItemContentQueryHandler
    : IQueryHandler<GetProjectItemContentQuery, Resource<AppItemModel>>
{
    private readonly IProjectItemContentRepository contentRepository;

    public GetProjectItemContentQueryHandler(
        IProjectItemContentRepository contentRepository
    )
    {
        this.contentRepository = contentRepository;
    }

    public async Task<Resource<AppItemModel>> Handle(
        GetProjectItemContentQuery query
        , CancellationToken cancellation
    )
    {
        var result = await contentRepository.GetProjectItemContent(filePathToWrite: query.ItemPath);

        if (result is not null)
        {
            result.Id = string.IsNullOrWhiteSpace(value: result.Id) ? Guid.NewGuid().ToString() : result.Id;

            return new Resource<AppItemModel>.Success(Data: result);
        }

        return new Resource<AppItemModel>.Error(Data: result
            , Message: $"Erro ao ler arquivo {Path.GetFileName(path: query.ItemPath)}");
    }
}