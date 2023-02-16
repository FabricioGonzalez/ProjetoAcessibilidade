using Common;

using Core.Entities.Solution.Explorer;

namespace Project.Application.Project.Contracts;
public interface IExplorerItemRepository
{
    Task<Resource<List<ExplorerItem>>> GetAllItemsAsync(string solutionPath);
    Task<Resource<ExplorerItem>> CreateExplorerItemAsync(ExplorerItem item);
    Task<Resource<ExplorerItem>> DeleteExplorerItemAsync(ExplorerItem item);
    Task<Resource<ExplorerItem>> RenameFolderItemAsync(ExplorerItem item);
    Task<Resource<ExplorerItem>> RenameFileItemAsync(ExplorerItem item);
    Task<Resource<ExplorerItem>> DeleteFolderItemAsync(ExplorerItem item);
    Task<Resource<ExplorerItem>> DeleteFileItemAsync(ExplorerItem item);
    Task<Resource<ExplorerItem>> UpdateExplorerItemAsync(ExplorerItem item);

    Resource<List<ExplorerItem>> GetAllItems(string solutionPath);
    Resource<ExplorerItem> CreateExplorerItem(ExplorerItem item);
    Resource<ExplorerItem> DeleteExplorerItem(ExplorerItem item);
    Resource<ExplorerItem> RenameFolderItem(ExplorerItem item);
    Resource<ExplorerItem> RenameFileItem(ExplorerItem item);
    Resource<ExplorerItem> DeleteFolderItem(ExplorerItem item);
    Resource<ExplorerItem> DeleteFileItem(ExplorerItem item);
    Resource<ExplorerItem> UpdateExplorerItem(ExplorerItem item);
}
