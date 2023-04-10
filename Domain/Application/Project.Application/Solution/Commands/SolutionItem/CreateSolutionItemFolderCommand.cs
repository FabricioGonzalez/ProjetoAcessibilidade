using Core.Entities.Solution.Explorer;
using Project.Domain.App.Models;
using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Solution.Commands.SolutionItem;

public sealed record CreateSolutionItemFolderCommand(
    string ItemName
    , string ItemPath
) : IRequest<Empty>;

public sealed class CreateSolutionItemFolderCommandHandler : ICommandHandler<CreateSolutionItemFolderCommand, Empty>
{
    private readonly IExplorerItemRepository _repository;

    public CreateSolutionItemFolderCommandHandler(
        IExplorerItemRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<Empty> Handle(
        CreateSolutionItemFolderCommand command
        , CancellationToken cancellation
    )
    {
        await _repository.RenameFolderItemAsync(item: new ExplorerItem
        {
            Name = command.ItemName, Path = command.ItemPath
        });

        return new Empty();
    }
}