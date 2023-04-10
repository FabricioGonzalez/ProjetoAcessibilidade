using Common;
using Core.Entities.Solution.Explorer;
using Project.Domain.App.Contracts;
using Project.Domain.Contracts;

namespace Project.Domain.App.Queries.Templates;

public sealed record GetAllTemplatesQuery : IRequest<Resource<List<ExplorerItem>>>;

public sealed class GetAllTemplatesQueryHandler : IQueryHandler<GetAllTemplatesQuery, Resource<List<ExplorerItem>>>
{
    private readonly IAppTemplateRepository repository;

    public GetAllTemplatesQueryHandler(
        IAppTemplateRepository repository
    )
    {
        this.repository = repository;
    }

    public async Task<Resource<List<ExplorerItem>>> Handle(
        GetAllTemplatesQuery query
        , CancellationToken cancellation
    )
    {
        var result = await repository.ReadAllTemplateItems();
        return result switch
        {
            Resource<List<ExplorerItem>>.Success item
                => new Resource<List<ExplorerItem>>.Success(Data: item.Data)
            , Resource<List<ExplorerItem>>.Error item
                => new Resource<List<ExplorerItem>>.Error(Message: item.Message, Data: item.Data)
            , _ => new Resource<List<ExplorerItem>>.Error(Message: "No Case matched", Data: null)
        };
    }
}