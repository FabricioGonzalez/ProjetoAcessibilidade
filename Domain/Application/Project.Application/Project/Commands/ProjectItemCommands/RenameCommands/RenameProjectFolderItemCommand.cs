using Common;

using Core.Entities.Solution.Explorer;

using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Commands.ProjectItemCommands.RenameCommands;

public sealed record RenameProjectFolderItemCommand(FolderItem Item) : IRequest<Resource<ExplorerItem>>;

public sealed class RenameProjectFolderItemCommandHandler : ICommandHandler<RenameProjectFolderItemCommand, Resource<ExplorerItem>>
{
    private readonly IExplorerItemRepository repository;

    public RenameProjectFolderItemCommandHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }
    public async Task<Resource<ExplorerItem>> Handle(RenameProjectFolderItemCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.RenameFolderItemAsync(request.Item);

        return result;
    }
}
