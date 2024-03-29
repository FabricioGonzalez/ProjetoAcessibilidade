﻿using LanguageExt.Common;
using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.FolderItems;

public sealed record RenameProjectFolderItemCommand(
    FolderItem Item
) : IRequest<Result<ExplorerItem>>;

public sealed class RenameProjectFolderItemCommandHandler
    : IHandler<RenameProjectFolderItemCommand, Result<ExplorerItem>>
{
    private readonly IExplorerItemRepository _repository;

    public RenameProjectFolderItemCommandHandler(
        IExplorerItemRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<Result<ExplorerItem>> HandleAsync(
        RenameProjectFolderItemCommand request
        , CancellationToken cancellationToken
    ) =>
        await _repository.RenameFolderItemAsync(request.Item);
}