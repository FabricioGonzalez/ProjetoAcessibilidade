using Common;
using ProjetoAcessibilidade.Domain.App.Models;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record CreateItemCommand(
    string ItemPath,
    string ItemName
) : IRequest<Resource<Empty>>;

public sealed class CreateItemCommandHandler : IHandler<CreateItemCommand, Resource<Empty>>
{
    public async Task<Resource<Empty>> HandleAsync(
        CreateItemCommand command,
        CancellationToken cancellation
    )
    {
        var result = await Locator.Current.GetService<IProjectItemContentRepository>()
            .GetSystemProjectItemContent(
                filePathToWrite: Path.Combine(
                    path1: Constants.AppItemsTemplateFolder,
                    path2: $"{command.ItemName}{Constants.AppProjectTemplateExtension}"));

        result.ItemName = Path.GetFileNameWithoutExtension(command.ItemPath);

        await Locator.Current.GetService<IProjectItemContentRepository>()
            .SaveProjectItemContent(
                dataToWrite: result,
                filePathToWrite: command.ItemPath);

        return new Resource<Empty>.Success(Data: Empty.Value);
    }
}