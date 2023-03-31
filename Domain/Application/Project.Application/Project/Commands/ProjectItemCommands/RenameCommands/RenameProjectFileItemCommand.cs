using Common;

using Core.Entities.Solution.Explorer;


using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Commands.ProjectItemCommands.RenameCommands;
public sealed record RenameProjectFileItemCommand(FileItem Item) : IRequest<Resource<ExplorerItem>>;

public sealed class RenameProjectFileItemCommandHandler : ICommandHandler<RenameProjectFileItemCommand, Resource<ExplorerItem>>
{
    private readonly IExplorerItemRepository repository;

    public RenameProjectFileItemCommandHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }
    public async Task<Resource<ExplorerItem>> Handle(RenameProjectFileItemCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.RenameFileItemAsync(request.Item);

        return result;
    }
}