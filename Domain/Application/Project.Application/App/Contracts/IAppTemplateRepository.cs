using App.Core.Entities.Solution.Explorer;

using Common;

namespace Project.Application.App.Contracts;
public interface IAppTemplateRepository
{
    Task<Resource<List<ExplorerItem>>> ReadAllTemplateItems();
}
