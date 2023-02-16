using Common;

using Core.Entities.Solution.Explorer;

using MediatR;

using Project.Application.Contracts;
using Project.Application.Project.Contracts;

namespace Project.Application.Project.Commands.ProjectItemCommands.RenameCommands;

public class RenameProjectFolderItemCommand : IRequest<Resource<ExplorerItem>>
{
    public RenameProjectFolderItemCommand(FolderItem item)
    {
        this.item = item;
    }
    public FolderItem item
    {
        get; init;
    }
}

public class RenameProjectFolderItemCommandHandler : ICommandHandler<RenameProjectFolderItemCommand, Resource<ExplorerItem>>
{
    private readonly IExplorerItemRepository repository;

    public RenameProjectFolderItemCommandHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }
    public async Task<Resource<ExplorerItem>> Handle(RenameProjectFolderItemCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.RenameFolderItemAsync(request.item);

        return result;
    }
}
