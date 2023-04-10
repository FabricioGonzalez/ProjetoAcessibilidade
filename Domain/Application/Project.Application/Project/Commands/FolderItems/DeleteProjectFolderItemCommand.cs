using Common;
using Core.Entities.Solution.Explorer;
using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Commands.FolderItems;

public sealed record DeleteProjectFolderItemCommand(
    FolderItem Item
) : IRequest<Resource<ExplorerItem>>;

public sealed class DeleteProjectFolderItemCommandHandler
    : ICommandHandler<DeleteProjectFolderItemCommand, Resource<ExplorerItem>>
{
    private readonly IExplorerItemRepository repository;

    public DeleteProjectFolderItemCommandHandler(
        IExplorerItemRepository repository
    )
    {
        this.repository = repository;
    }

    public async Task<Resource<ExplorerItem>> Handle(
        DeleteProjectFolderItemCommand request
        , CancellationToken cancellationToken
    )
    {
        var result = await repository.DeleteFolderItemAsync(item: request.Item);

        return result;
    }
}