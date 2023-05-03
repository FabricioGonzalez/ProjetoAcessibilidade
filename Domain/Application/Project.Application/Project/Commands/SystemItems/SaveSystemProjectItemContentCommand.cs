using Common;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Domain.App.Models;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Project.Commands.SystemItems;

public sealed record SaveSystemProjectItemContentCommand(
    AppItemModel AppItem
    , string ItemPath
) : IRequest<Resource<Empty>>;

public class SaveSystemProjectItemContentCommandHandler
    : IHandler<SaveSystemProjectItemContentCommand, Resource<Empty>>
{
    public async Task<Resource<Empty>> HandleAsync(
        SaveSystemProjectItemContentCommand command
        , CancellationToken cancellation
    )
    {
        await Locator.Current.GetService<IProjectItemContentRepository>().SaveSystemProjectItemContentSerealizer(
            dataToWrite: command.AppItem
            , filePathToWrite: command.ItemPath);

        return new Resource<Empty>.Success(Data: Empty.Value);
    }
}