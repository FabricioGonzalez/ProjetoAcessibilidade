using Common.Models;
using Common.Result;

using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record DeleteProjectFileItemCommand(
    string itemPath
) : IRequest<Result<Empty>>;

public sealed class DeleteProjectFileItemCommandHandler
    : IHandler<DeleteProjectFileItemCommand, Result<Empty>>
{
    private readonly IExplorerItemRepository _repository;

    public DeleteProjectFileItemCommandHandler(IExplorerItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Empty>> HandleAsync(
        DeleteProjectFileItemCommand request
        , CancellationToken cancellationToken
    )
    {
        return (await _repository.DeleteFileItemAsync(itemPath: request.itemPath))
            .Match(
            success => Result<Empty>.Success(new Empty()),
            failure => Result<Empty>.Failure(new("No data was Deleted"))
            );
    }
}