using Common.Result;

using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.FolderItems;

public sealed record DeleteProjectFolderItemCommand(
    string ItemPath
) : IRequest<Result<ExplorerItem, Exception>>;

public sealed class DeleteProjectFolderItemCommandHandler
    : IHandler<DeleteProjectFolderItemCommand, Result<ExplorerItem, Exception>>
{
    private IExplorerItemRepository _repository;

    public DeleteProjectFolderItemCommandHandler(IExplorerItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ExplorerItem, Exception>> HandleAsync(
        DeleteProjectFolderItemCommand request
        , CancellationToken cancellationToken
    )
    {
        return await _repository.DeleteFolderItemAsync(request.ItemPath);
    }
}