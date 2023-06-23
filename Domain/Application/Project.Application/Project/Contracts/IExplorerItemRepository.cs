using Common.Result;

using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.App.Models;

namespace ProjetoAcessibilidade.Domain.Project.Contracts;

public interface IExplorerItemRepository
{
    public Result<ExplorerItem, Exception> CreateExplorerItem(
          ExplorerItem item
      );

    public Result<ExplorerItem, Exception> DeleteExplorerItem(
        ExplorerItem item
    );

    public Result<ExplorerItem, Exception> RenameFileItem(
        ExplorerItem item
    );

    public Result<ExplorerItem, Exception> DeleteFolderItem(
        ExplorerItem item
    );

    public Result<IEnumerable<ExplorerItem>, Exception> GetAllItems(
        string solutionPath
    );

    public Task<Result<IEnumerable<ExplorerItem>, Exception>> GetAllItemsAsync(
        string solutionPath
    );

    public Result<ExplorerItem, Exception> UpdateExplorerItem(
        ExplorerItem item
    );

    public Task<Result<ExplorerItem, Exception>> UpdateExplorerItemAsync(
        ExplorerItem item
    );

    public Task<Result<ExplorerItem, Exception>> RenameFileItemAsync(
        ExplorerItem item
    );

    public Task<Result<ExplorerItem, Exception>> RenameFolderItemAsync(
        ExplorerItem item
    );

    public Task<Result<ExplorerItem, Exception>> DeleteFolderItemAsync(
        string itemPath
    );

    public Task<Result<ExplorerItem, Exception>> DeleteFileItemAsync(
        string itemPath
    );

    public Result<ExplorerItem, Exception> RenameFolderItem(
        ExplorerItem item
    );

    public Result<ExplorerItem, Exception> DeleteFileItem(
        ExplorerItem item
    );

    public Result<Empty, Exception> RenameFolderItem(
        string itemName,
        string itemPath
    );

    public Task<Result<ExplorerItem, Exception>> CreateExplorerItemAsync(
        ExplorerItem item
    );

    public Task<Result<ExplorerItem, Exception>> DeleteExplorerItemAsync(
        ExplorerItem item
    );

}