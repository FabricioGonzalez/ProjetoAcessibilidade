using Common;
using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;
using ProjetoAcessibilidade.Domain.App.Contracts;
using ProjetoAcessibilidade.Domain.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.App.Queries.Templates;

public sealed record GetAllTemplatesQuery : IRequest<Resource<List<ItemModel>>>;

public sealed class GetAllTemplatesQueryHandler : IHandler<GetAllTemplatesQuery, Resource<List<ItemModel>>>
{
    public async Task<Resource<List<ItemModel>>> HandleAsync(
        GetAllTemplatesQuery query
        , CancellationToken cancellation
    )
    {
        var result = await Locator.Current.GetService<IAppTemplateRepository>().ReadAllTemplateItems();
        return result switch
        {
            Resource<List<ItemModel>>.Success item
                => new Resource<List<ItemModel>>.Success(Data: item.Data),
            Resource<List<ItemModel>>.Error item
                => new Resource<List<ItemModel>>.Error(Message: item.Message, Data: item.Data),
            _ => new Resource<List<ItemModel>>.Error(Message: "No Case matched", Data: null)
        };
    }
}