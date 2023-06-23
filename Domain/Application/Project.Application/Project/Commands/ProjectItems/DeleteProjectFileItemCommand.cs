using Common.Result;

using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.App.Models;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record DeleteProjectFileItemCommand(
    string itemPath
) : IRequest<Result<Empty, Exception>>;

public sealed class DeleteProjectFileItemCommandHandler
    : IHandler<DeleteProjectFileItemCommand, Result<Empty, Exception>>
{
    private IExplorerItemRepository _repository;

    public DeleteProjectFileItemCommandHandler(IExplorerItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Empty, Exception>> HandleAsync(
        DeleteProjectFileItemCommand request
        , CancellationToken cancellationToken
    )
    {
        Result<ExplorerItem, Exception> result = await _repository.DeleteFileItemAsync(itemPath: request.itemPath);

        return result.Match(
            success => Result<Empty, Exception>.Success(new Empty()),
            failure => Result<Empty, Exception>.Failure(new("No data was Deleted"))
            );
    }
}