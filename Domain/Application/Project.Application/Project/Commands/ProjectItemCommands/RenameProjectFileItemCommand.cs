using App.Core.Entities.Solution.Explorer;

using Common;

using MediatR;
using Project.Application.Project.Contracts;

namespace Project.Application.Project.Commands.ProjectItemCommands;
public class RenameProjectFileItemCommand : IRequest<FileItem>
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

public class RenameProjectFileItemCommandHandler
{
    private readonly IExplorerItemRepository repository;

    public RenameProjectFileItemCommandHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }
    public async Task<ExplorerItem> Handle(RenameProjectFileItemCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.RenameExplorerItemAsync(request.item);

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