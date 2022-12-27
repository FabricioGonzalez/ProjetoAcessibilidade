using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App.Core.Entities.Solution.Project.AppItem;

using Common;

using MediatR;

using Project.Application.Contracts;

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
    public GetProjectItemContentQueryHandler()
    {

    }

    public Task<Resource<AppItemModel>> Handle(GetProjectItemContentQuery query, CancellationToken cancellation)
    {
        return default;
    }
}
