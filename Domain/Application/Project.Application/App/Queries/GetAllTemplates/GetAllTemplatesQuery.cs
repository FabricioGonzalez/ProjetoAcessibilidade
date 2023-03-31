﻿using Common;

using Core.Entities.Solution.Explorer;

using Project.Domain.App.Contracts;
using Project.Domain.Contracts;

namespace Project.Domain.App.Queries.GetAllTemplates;
public sealed record GetAllTemplatesQuery : IRequest<Resource<List<ExplorerItem>>>
{

}

public sealed class GetAllTemplatesQueryHandler : IQueryHandler<GetAllTemplatesQuery, Resource<List<ExplorerItem>>>
{

    private readonly IAppTemplateRepository repository;
    public GetAllTemplatesQueryHandler(IAppTemplateRepository repository)
    {
        this.repository = repository;
    }
    public async Task<Resource<List<ExplorerItem>>> Handle(GetAllTemplatesQuery query, CancellationToken cancellation)
    {
        var result = await repository.ReadAllTemplateItems();
        if (result is not null)
        {
            return result switch
            {
                Resource<List<ExplorerItem>>.Success item
                => new Resource<List<ExplorerItem>>.Success(item.Data),

                Resource<List<ExplorerItem>>.Error item
                => new Resource<List<ExplorerItem>>.Error(item.Message, item.Data),

                Resource<List<ExplorerItem>>.IsLoading item
                => new Resource<List<ExplorerItem>>.IsLoading(item.Data, item.isLoading),
            };
        }
        return new Resource<List<ExplorerItem>>.Error("Algo deu errado", null);
    }
}

