using Common;

using Core.Entities.Solution.Explorer;

using MediatR;

using Project.Application.Contracts;
using Project.Application.Project.Contracts;

namespace Project.Application.Project.Commands.ProjectItemCommands.RenameCommands;
public class RenameProjectFileItemCommand : IRequest<Resource<ExplorerItem>>
{
    public RenameProjectFileItemCommand(FileItem item)
    {
        this.item = item;
    }
    public FileItem item
    {
        get; init;
    }
}

public class RenameProjectFileItemCommandHandler : ICommandHandler<RenameProjectFileItemCommand, Resource<ExplorerItem>>
{
    private readonly IExplorerItemRepository repository;

    public RenameProjectFileItemCommandHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }
    public async Task<Resource<ExplorerItem>> Handle(RenameProjectFileItemCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.RenameFileItemAsync(request.item);

        return result;
    }
}