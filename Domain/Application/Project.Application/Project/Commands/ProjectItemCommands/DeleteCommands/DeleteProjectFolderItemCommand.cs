using App.Core.Entities.Solution.Explorer;

using Common;

using MediatR;

using Project.Application.Contracts;
using Project.Application.Project.Contracts;

namespace Project.Application.Project.Commands.ProjectItemCommands.DeleteCommands;
public class DeleteProjectFolderItemCommand : IRequest<Resource<ExplorerItem>>
{
    public DeleteProjectFolderItemCommand(FolderItem item)
    {
        this.item = item;
    }
    public FolderItem item
    {
        get; init;
    }
}

public class DeleteProjectFolderItemCommandHandler : ICommandHandler<DeleteProjectFolderItemCommand, Resource<ExplorerItem>>
{
    private readonly IExplorerItemRepository repository;

    public DeleteProjectFolderItemCommandHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }
    public async Task<Resource<ExplorerItem>> Handle(DeleteProjectFolderItemCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.DeleteFolderItemAsync(request.item);

        return result;
    }
}
