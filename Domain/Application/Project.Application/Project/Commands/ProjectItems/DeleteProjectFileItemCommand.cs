using Common;
using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.App.Models;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record DeleteProjectFileItemCommand(
    FileItem Item
) : IRequest<Resource<Empty>>;

public sealed class DeleteProjectFileItemCommandHandler
    : IHandler<DeleteProjectFileItemCommand, Resource<Empty>>
{
    public async Task<Resource<Empty>> HandleAsync(
        DeleteProjectFileItemCommand request
        , CancellationToken cancellationToken
    )
    {
        var result = await Locator.Current.GetService<IExplorerItemRepository>()
            .DeleteFileItemAsync(item: request.Item);

        return result switch
        {
            Resource<ExplorerItem>.Success => new Resource<Empty>.Success(Data: new Empty()),
            Resource<ExplorerItem>.Error => new Resource<Empty>.Error(Data: new Empty()
                , Message: "No data was Deleted"),
            _ => new Resource<Empty>.Error(Data: new Empty()
                , Message: $"Nothing was returned from {nameof(IExplorerItemRepository.DeleteFileItemAsync)}")
        };
    }
}