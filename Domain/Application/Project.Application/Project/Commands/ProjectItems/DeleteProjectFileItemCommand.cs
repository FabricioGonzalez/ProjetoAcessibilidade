using Common;
using Core.Entities.Solution.Explorer;
using Project.Domain.App.Models;
using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Commands.ProjectItems;

public sealed record DeleteProjectFileItemCommand(
    FileItem Item
) : IRequest<Resource<Empty>>;

public sealed class DeleteProjectFileItemCommandHandler
    : ICommandHandler<DeleteProjectFileItemCommand, Resource<Empty>>
{
    private readonly IExplorerItemRepository repository;

    public DeleteProjectFileItemCommandHandler(
        IExplorerItemRepository repository
    )
    {
        this.repository = repository;
    }

    public async Task<Resource<Empty>> Handle(
        DeleteProjectFileItemCommand request
        , CancellationToken cancellationToken
    )
    {
        var result = await repository.DeleteFileItemAsync(item: request.Item);

        return result switch
        {
            Resource<ExplorerItem>.Success => new Resource<Empty>.Success(Data: new Empty())
            , Resource<ExplorerItem>.Error => new Resource<Empty>.Error(Data: new Empty()
                , Message: "No data was Deleted")
            , _ => new Resource<Empty>.Error(Data: new Empty()
                , Message: $"Nothing was returned from {nameof(repository.DeleteFileItemAsync)}")
        };
    }
}