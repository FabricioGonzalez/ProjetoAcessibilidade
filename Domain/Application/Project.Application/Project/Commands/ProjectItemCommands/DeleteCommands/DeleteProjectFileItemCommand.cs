using Common;

using Core.Entities.Solution.Explorer;

using MediatR;

using Project.Application.Contracts;
using Project.Application.Project.Contracts;

namespace Project.Application.Project.Commands.ProjectItemCommands.DeleteCommands;
public class DeleteProjectFileItemCommand : IRequest<Resource<ExplorerItem>>
{
    public DeleteProjectFileItemCommand(FileItem item)
    {
        this.item = item;
    }
    public FileItem item
    {
        get; init;
    }
}

public class DeleteProjectFileItemCommandHandler : ICommandHandler<DeleteProjectFileItemCommand, Resource<ExplorerItem>>
{
    private readonly IExplorerItemRepository repository;

    public DeleteProjectFileItemCommandHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }
    public async Task<Resource<ExplorerItem>> Handle(DeleteProjectFileItemCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.DeleteFileItemAsync(request.item);

        return result;
    }
}
