using Common.Result;

using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.App.Models;

namespace ProjetoAcessibilidade.Domain.Project.Contracts;

public interface IExplorerItemRepository
{
    public Result<ExplorerItem> CreateExplorerItem(
          ExplorerItem item
      );

    public Result<ExplorerItem> DeleteExplorerItem(
        ExplorerItem item
    );

    public Result<ExplorerItem> RenameFileItem(
        ExplorerItem item
    );

    public Result<ExplorerItem> DeleteFolderItem(
        ExplorerItem item
    );

    public Result<IEnumerable<ExplorerItem>> GetAllItems(
        string solutionPath
    );

    public Task<Result<IEnumerable<ExplorerItem>>> GetAllItemsAsync(
        string solutionPath
    );

    public Result<ExplorerItem> UpdateExplorerItem(
        ExplorerItem item
    );

    public Task<Result<ExplorerItem>> UpdateExplorerItemAsync(
        ExplorerItem item
    );

    public Task<Result<ExplorerItem>> RenameFileItemAsync(
        ExplorerItem item
    );

    public Task<Result<ExplorerItem>> RenameFolderItemAsync(
        ExplorerItem item
    );

    public Task<Result<ExplorerItem>> DeleteFolderItemAsync(
        string itemPath
    );

    public Task<Result<ExplorerItem>> DeleteFileItemAsync(
        string itemPath
    );

    public Result<ExplorerItem> RenameFolderItem(
        ExplorerItem item
    );

    public Result<ExplorerItem> DeleteFileItem(
        ExplorerItem item
    );

    public Result<Empty> RenameFolderItem(
        string itemName,
        string itemPath
    );

    public Task<Result<ExplorerItem>> CreateExplorerItemAsync(
        ExplorerItem item
    );

    public Task<Result<ExplorerItem>> DeleteExplorerItemAsync(
        ExplorerItem item
    );

}