using Common;

using Core.Entities.Solution.Project.AppItem;

using MediatR;

using Project.Application.Contracts;
using Project.Application.Project.Contracts;

namespace Project.Application.Project.Queries.GetProjectItemContent;
public class GetProjectItemContentQuery : IRequest<AppItemModel>
{
    public GetProjectItemContentQuery(string itemPath)
    {
        ItemPath = itemPath;
    }

    public string ItemPath
    {
        get; set;
    }
}

public class GetProjectItemContentQueryHandler : IQueryHandler<GetProjectItemContentQuery, Resource<AppItemModel>>
{
    private readonly IProjectItemContentRepository contentRepository;
    public GetProjectItemContentQueryHandler(IProjectItemContentRepository contentRepository)
    {
        this.contentRepository = contentRepository;
    }

    public async Task<Resource<AppItemModel>> Handle(GetProjectItemContentQuery query, CancellationToken cancellation)
    {
        var result = await contentRepository.GetProjectItemContent(query.ItemPath);

        if (result is not null)
        {
            return new Resource<AppItemModel>.Success(Data: result);
        }
        return new Resource<AppItemModel>.Error(Data: result, Message: $"Erro ao ler arquivo {Path.GetFileName(query.ItemPath)}");
    }
}
