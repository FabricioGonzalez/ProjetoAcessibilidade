using Common;
using Core.Entities.Solution.Explorer;
using Core.Entities.Solution.ItemsGroup;
using Project.Domain.App.Contracts;
using Project.Domain.Contracts;

namespace Project.Domain.App.Queries.Templates;

public sealed record GetAllTemplatesQuery : IRequest<Resource<List<ItemModel>>>;

public sealed class GetAllTemplatesQueryHandler : IQueryHandler<GetAllTemplatesQuery, Resource<List<ItemModel>>>
{
    private readonly IAppTemplateRepository repository;

    public GetAllTemplatesQueryHandler(
        IAppTemplateRepository repository
    )
    {
        this.repository = repository;
    }

    public async Task<Resource<List<ItemModel>>> Handle(
        GetAllTemplatesQuery query
        , CancellationToken cancellation
    )
    {
        var result = await repository.ReadAllTemplateItems();
        return result switch
        {
            Resource<List<ItemModel>>.Success item
                => new Resource<List<ItemModel>>.Success(Data: item.Data)
            , Resource<List<ItemModel>>.Error item
                => new Resource<List<ItemModel>>.Error(Message: item.Message, Data: item.Data)
            , _ => new Resource<List<ItemModel>>.Error(Message: "No Case matched", Data: null)
        };
    }
}