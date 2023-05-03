using Common;
using ProjetoAcessibilidade.Domain.App.Models;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;
using Splat;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record CreateItemCommand(
    string ItemPath
    , string ItemName
) : IRequest<Resource<Empty>>;

public sealed class CreateItemCommandHandler : IHandler<CreateItemCommand, Resource<Empty>>
{
    private readonly IProjectItemContentRepository contentRepository;

    public CreateItemCommandHandler(
        IProjectItemContentRepository content
    )
    {
        contentRepository = content;
    }

    public async Task<Resource<Empty>> HandleAsync(
        CreateItemCommand command
        , CancellationToken cancellation
    )
    {
        await Locator.Current.GetService<IProjectItemContentRepository>().SaveProjectItemContent(
            dataToWrite: await Locator.Current.GetService<IProjectItemContentRepository>()
                .GetSystemProjectItemContent(
                    filePathToWrite: Path.Combine(
                        path1: Constants.AppItemsTemplateFolder
                        , path2: $"{command.ItemName}{Constants.AppProjectTemplateExtension}")),
            filePathToWrite: command.ItemPath);

        return new Resource<Empty>.Success(Data: Empty.Value);
    }
}