using Common.Result;

using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record RenameProjectFileItemCommand(
    FileItem Item
) : IRequest<Result<ExplorerItem, Exception>>;

public sealed class RenameProjectFileItemCommandHandler
    : IHandler<RenameProjectFileItemCommand, Result<ExplorerItem, Exception>>
{
    private IExplorerItemRepository _repository;

    public RenameProjectFileItemCommandHandler(IExplorerItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ExplorerItem, Exception>> HandleAsync(
        RenameProjectFileItemCommand request
        , CancellationToken cancellationToken
    )
    {
        return await _repository.RenameFileItemAsync(item: request.Item);


    }
}