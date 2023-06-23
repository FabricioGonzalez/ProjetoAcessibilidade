using Common.Result;

using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.FolderItems;

public sealed record RenameProjectFolderItemCommand(
    FolderItem Item
) : IRequest<Result<ExplorerItem, Exception>>;

public sealed class RenameProjectFolderItemCommandHandler
    : IHandler<RenameProjectFolderItemCommand, Result<ExplorerItem, Exception>>
{

    private IExplorerItemRepository _repository;
    public RenameProjectFolderItemCommandHandler(IExplorerItemRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<ExplorerItem, Exception>> HandleAsync(
        RenameProjectFolderItemCommand request
        , CancellationToken cancellationToken
    )
    {
        return await _repository.RenameFolderItemAsync(item: request.Item);
    }
}