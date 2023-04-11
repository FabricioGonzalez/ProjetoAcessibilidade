using Common;
using Core.Entities.Solution.Explorer;
using Core.Entities.Solution.ItemsGroup;

namespace Project.Domain.App.Contracts;

public interface IAppTemplateRepository
{
    Task<Resource<List<ItemModel>>> ReadAllTemplateItems();
}