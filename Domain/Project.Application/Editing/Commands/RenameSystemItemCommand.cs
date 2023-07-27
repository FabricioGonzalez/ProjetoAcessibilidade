using LanguageExt.Common;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Editing.Commands;

public sealed record RenameSystemItemCommand(
    string Name
    , string Path
) : IRequest<Result<string>>;

public sealed class RenameSystemItemCommandHandler
    : IHandler<RenameSystemItemCommand, Result<string>>
{
    private readonly IExplorerItemRepository _repository;

    public RenameSystemItemCommandHandler(
        IExplorerItemRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<Result<string>> HandleAsync(
        RenameSystemItemCommand request
        , CancellationToken cancellationToken
    ) =>
        await _repository.RenameSystemItemAsync(request.Name, request.Path);
}