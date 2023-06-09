using ProjetoAcessibilidade.Domain.App.Models;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Solution.Commands.SolutionItem;

public sealed record CreateSolutionItemFolderCommand(
    string ItemName
    , string ItemPath
) : IRequest<Empty>;

public sealed class CreateSolutionItemFolderCommandHandler : IHandler<CreateSolutionItemFolderCommand, Empty>
{
    private readonly IExplorerItemRepository _repository;

    public CreateSolutionItemFolderCommandHandler(
        IExplorerItemRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<Empty> HandleAsync(
        CreateSolutionItemFolderCommand command
        , CancellationToken cancellation
    )
    {
        _repository.RenameFolderItem(itemName: command.ItemName, itemPath: command.ItemPath);

        return new Empty();
    }
}