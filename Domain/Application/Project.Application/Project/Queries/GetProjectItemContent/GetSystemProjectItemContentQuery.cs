using Common;

using Core.Entities.Solution.Project.AppItem;

using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Queries.GetProjectItemContent;
public sealed record GetSystemProjectItemContentQuery(string ItemPath) : IRequest<Resource<AppItemModel>>;

public sealed class GetSystemProjectItemContentQueryHandler : IQueryHandler<GetSystemProjectItemContentQuery, Resource<AppItemModel>>
{
    private readonly IProjectItemContentRepository contentRepository;
    public GetSystemProjectItemContentQueryHandler(IProjectItemContentRepository contentRepository)
    {
        this.contentRepository = contentRepository;
    }

    public async Task<Resource<AppItemModel>> Handle(GetSystemProjectItemContentQuery query, CancellationToken cancellation)
    {
        var result = await contentRepository.GetSystemProjectItemContent(query.ItemPath);

        if (result is not null)
        {
            return new Resource<AppItemModel>.Success(Data: result);
        }
        return new Resource<AppItemModel>.Error(Data: result, Message: $"Erro ao ler arquivo {Path.GetFileName(query.ItemPath)}");
    }
}
