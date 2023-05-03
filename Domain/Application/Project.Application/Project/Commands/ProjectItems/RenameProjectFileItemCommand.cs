using Common;
using ProjetoAcessibilidade.Core.Entities.Solution.Explorer;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record RenameProjectFileItemCommand(
    FileItem Item
) : IRequest<Resource<ExplorerItem>>;

public sealed class RenameProjectFileItemCommandHandler
    : IHandler<RenameProjectFileItemCommand, Resource<ExplorerItem>>
{
    public async Task<Resource<ExplorerItem>> HandleAsync(
        RenameProjectFileItemCommand request
        , CancellationToken cancellationToken
    )
    {
        var result = await Locator.Current.GetService<IExplorerItemRepository>()
            .RenameFileItemAsync(item: request.Item);

        return result;
    }
}