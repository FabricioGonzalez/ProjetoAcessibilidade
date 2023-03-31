using Common;

using Core.Entities.Solution.Explorer;

using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Commands.ProjectItemCommands.DeleteCommands;
public sealed record DeleteProjectFileItemCommand(FileItem Item) : IRequest<Resource<ExplorerItem>>;

public sealed class DeleteProjectFileItemCommandHandler : ICommandHandler<DeleteProjectFileItemCommand, Resource<ExplorerItem>>
{
    private readonly IExplorerItemRepository repository;

    public DeleteProjectFileItemCommandHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }
    public async Task<Resource<ExplorerItem>> Handle(DeleteProjectFileItemCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.DeleteFileItemAsync(request.Item);

        return result;
    }
}
