using App.Core.Entities.Solution.Explorer;

using Common;

using MediatR;
using Project.Application.Project.Contracts;

namespace Project.Application.Project.Commands.ProjectItemCommands;
public class DeleteProjectFolderItemCommand : IRequest<FolderItem>
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

public class DeleteProjectFolderItemCommandHandler
{
    private readonly IExplorerItemRepository repository;

    public DeleteProjectFolderItemCommandHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }
    public async Task<ExplorerItem> Handle(RenameProjectFolderItemCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.DeleteExplorerItemAsync(request.item);

        result
            .OnError(out var res, out var message)
            .OnLoading(out res, out var isLoading)
            .OnSuccess(out res);

        if (res is not null)
        {
            return res;
        }

        return new();
    }
}
