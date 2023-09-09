using Common.Models;
using LanguageExt.Common;
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

    public DeleteProjectFileItemCommandHandler(
        IExplorerItemRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<Result<Empty>> HandleAsync(
        DeleteProjectFileItemCommand request
        , CancellationToken cancellationToken
    ) =>
        (await _repository.DeleteFileItemAsync(request.itemPath))
        .Match(
            success => new Result<Empty>(new Empty()),
            failure => new Result<Empty>(new Exception("No data was Deleted"))
        );
}