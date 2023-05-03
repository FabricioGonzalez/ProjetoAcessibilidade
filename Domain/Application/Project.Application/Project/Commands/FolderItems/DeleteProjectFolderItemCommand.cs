using Common;
using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Project.Commands.FolderItems;

public sealed record DeleteProjectFolderItemCommand(
    FolderItem Item
) : IRequest<Resource<ExplorerItem>>;

public sealed class DeleteProjectFolderItemCommandHandler
    : IHandler<DeleteProjectFolderItemCommand, Resource<ExplorerItem>>
{
    public async Task<Resource<ExplorerItem>> HandleAsync(
        DeleteProjectFolderItemCommand request
        , CancellationToken cancellationToken
    )
    {
        var result = await Locator.Current.GetService<IExplorerItemRepository>()
            .DeleteFolderItemAsync(item: request.Item);

        return result;
    }
}