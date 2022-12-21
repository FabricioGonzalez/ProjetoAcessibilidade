using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App.Core.Entities.Solution.Explorer;

using Common;

using MediatR;
using Project.Application.Project.Contracts;

namespace Project.Application.Project.Commands.ProjectItemCommands;
public class DeleteProjectFileItemCommand : IRequest<FileItem>
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

public class DeleteProjectFileItemCommandHandler
{
    private readonly IExplorerItemRepository repository;

    public DeleteProjectFileItemCommandHandler(IExplorerItemRepository repository)
    {
        this.repository = repository;
    }
    public async Task<ExplorerItem> Handle(DeleteProjectFileItemCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.DeleteFileItemAsync(request.item);

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
