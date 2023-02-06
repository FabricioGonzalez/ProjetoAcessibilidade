using Common;

using Core.Entities.Solution.Explorer;

namespace Project.Application.App.Contracts;
public interface IAppTemplateRepository
{
    Task<Resource<List<ExplorerItem>>> ReadAllTemplateItems();
}
