using Common;
using Project.Domain.App.Models;
using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Commands.ProjectItems;

public sealed record CreateItemCommand(
    string ItemPath
    , string ItemName
) : IRequest<Resource<Empty>>;

public sealed class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, Resource<Empty>>
{
    private readonly IProjectItemContentRepository contentRepository;

    public CreateItemCommandHandler(
        IProjectItemContentRepository content
    )
    {
        contentRepository = content;
    }

    public async Task<Resource<Empty>> Handle(
        CreateItemCommand command
        , CancellationToken cancellation
    )
    {
        await contentRepository.SaveProjectItemContent(
            dataToWrite: await contentRepository
                .GetSystemProjectItemContent(
                    filePathToWrite: Path.Combine(
                        path1: Constants.AppItemsTemplateFolder
                        , path2: $"{command.ItemName}{Constants.AppProjectTemplateExtension}")),
            filePathToWrite: command.ItemPath);

        return new Resource<Empty>.Success(Data: Empty.Value);
    }
}