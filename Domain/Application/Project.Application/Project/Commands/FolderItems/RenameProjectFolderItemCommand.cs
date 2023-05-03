using Common;
using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Project.Commands.FolderItems;

public sealed record RenameProjectFolderItemCommand(
    FolderItem Item
) : IRequest<Resource<ExplorerItem>>;

public sealed class RenameProjectFolderItemCommandHandler
    : IHandler<RenameProjectFolderItemCommand, Resource<ExplorerItem>>
{
    public async Task<Resource<ExplorerItem>> HandleAsync(
        RenameProjectFolderItemCommand request
        , CancellationToken cancellationToken
    )
    {
        var result = await Locator.Current.GetService<IExplorerItemRepository>()
            .RenameFolderItemAsync(item: request.Item);

        return result;
    }
}