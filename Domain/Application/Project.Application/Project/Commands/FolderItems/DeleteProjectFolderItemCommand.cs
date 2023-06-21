using Common;

using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.FolderItems;

public sealed record DeleteProjectFolderItemCommand(
    string ItemPath
) : IRequest<Resource<ExplorerItem>>;

public sealed class DeleteProjectFolderItemCommandHandler
    : IHandler<DeleteProjectFolderItemCommand, Resource<ExplorerItem>>
{
    private IExplorerItemRepository _repository;

    public DeleteProjectFolderItemCommandHandler(IExplorerItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Resource<ExplorerItem>> HandleAsync(
        DeleteProjectFolderItemCommand request
        , CancellationToken cancellationToken
    )
    {
        var result = await _repository.DeleteFolderItemAsync(request.ItemPath);

        return result;
    }
}