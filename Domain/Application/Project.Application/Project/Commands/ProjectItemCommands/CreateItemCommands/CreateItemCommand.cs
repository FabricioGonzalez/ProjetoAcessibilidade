using Common;

using Project.Domain.App.Models;
using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Commands.ProjectItemCommands.CreateItemCommands;
public sealed record CreateItemCommand(string ItemPath, string ItemName) : IRequest<Resource<Empty>>;

public sealed class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, Resource<Empty>>
{
    private readonly IProjectItemContentRepository contentRepository;
    public CreateItemCommandHandler(IProjectItemContentRepository content)
    {
        contentRepository = content;
    }
    public async Task<Resource<Empty>> Handle(CreateItemCommand command, CancellationToken cancellation)
    {
        await contentRepository.SaveProjectItemContent(
            await contentRepository
            .GetSystemProjectItemContent(
            Path.Combine(
                Constants.AppItemsTemplateFolder, $"{command.ItemName}{Constants.AppProjectTemplateExtension}")),
            command.ItemPath);

        return new Resource<Empty>.Success(Empty.Value);
    }
}
