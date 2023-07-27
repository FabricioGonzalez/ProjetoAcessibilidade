using Common;
using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;

namespace ProjetoAcessibilidade.Domain.App.Contracts;

public interface IAppTemplateRepository
{
    Task<Resource<List<ItemModel>>> ReadAllTemplateItems();
}