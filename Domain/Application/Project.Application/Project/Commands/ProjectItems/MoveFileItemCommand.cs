using Common.Models;
using Common.Result;

using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;
public sealed record MoveFileItemCommand(string OldPath, string NewPath) : IRequest<Result<Empty>>;

public sealed class MoveFileItemCommandHandler : IHandler<MoveFileItemCommand, Result<Empty>>
{
    private readonly IExplorerItemRepository _repository;
    public MoveFileItemCommandHandler(IExplorerItemRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<Empty>> HandleAsync(MoveFileItemCommand query, CancellationToken cancellation)
    {
        return await Task.Run(() => _repository.MoveFileItem(query.OldPath, query.NewPath));
    }
}
