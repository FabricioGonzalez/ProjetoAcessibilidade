﻿using Common.Result;

using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record RenameProjectFileItemCommand(
    FileItem Item
) : IRequest<Result<ExplorerItem>>;

public sealed class RenameProjectFileItemCommandHandler
    : IHandler<RenameProjectFileItemCommand, Result<ExplorerItem>>
{
    private readonly IExplorerItemRepository _repository;

    public RenameProjectFileItemCommandHandler(IExplorerItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ExplorerItem>> HandleAsync(
        RenameProjectFileItemCommand request
        , CancellationToken cancellationToken
    )
    {
        return await _repository.RenameFileItemAsync(item: request.Item);


    }
}