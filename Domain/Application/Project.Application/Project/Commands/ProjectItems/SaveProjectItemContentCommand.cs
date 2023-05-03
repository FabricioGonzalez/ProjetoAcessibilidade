using Common;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Domain.App.Models;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record SaveProjectItemContentCommand(
    AppItemModel AppItem
    , string ItemPath
) : IRequest<Resource<Empty>>;

public sealed class SaveProjectItemContentCommandHandler
    : IHandler<SaveProjectItemContentCommand, Resource<Empty>>
{
    public async Task<Resource<Empty>> HandleAsync(
        SaveProjectItemContentCommand command
        , CancellationToken cancellation
    )
    {
        await Locator.Current.GetService<IProjectItemContentRepository>()
            .SaveProjectItemContent(dataToWrite: command.AppItem, filePathToWrite: command.ItemPath);

        return new Resource<Empty>.Success(Data: Empty.Value);
    }
}