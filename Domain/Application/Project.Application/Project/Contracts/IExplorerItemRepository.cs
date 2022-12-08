using App.Core.Entities.Solution.Explorer;

using Common;

namespace Project.Application.Project.Contracts;
public interface IExplorerItemRepository
{
    Task<Resource<List<ExplorerItem>>> GetAllItemsAsync(string solutionPath);
    Task<Resource<ExplorerItem>> CreateExplorerItemAsync(ExplorerItem item);
    Task<Resource<ExplorerItem>> DeleteExplorerItemAsync(ExplorerItem item);
    Task<Resource<ExplorerItem>> RenameExplorerItemAsync(ExplorerItem item);
    Task<Resource<ExplorerItem>> UpdateExplorerItemAsync(ExplorerItem item);  
    
    Resource<List<ExplorerItem>> GetAllItems(string solutionPath);
    Resource<ExplorerItem> CreateExplorerItem(ExplorerItem item);
    Resource<ExplorerItem> DeleteExplorerItem(ExplorerItem item);
    Resource<ExplorerItem> RenameExplorerItem(ExplorerItem item);
    Resource<ExplorerItem> UpdateExplorerItem(ExplorerItem item);
}
