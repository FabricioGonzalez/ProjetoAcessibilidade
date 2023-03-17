using Common;

using MediatR;

using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Commands.ProjectItemCommands.CreateItemCommands;
public class CreateItemCommand : IRequest<Unit>
{
    public string ItemPath
    {
        get;
    }
    public string ItemName
    {
        get;
    }
    public CreateItemCommand(string path, string itemName)
    {
        ItemPath = path;
        ItemName = itemName;

    }
}
public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, Resource<Unit>>
{
    private readonly IProjectItemContentRepository contentRepository;
    public CreateItemCommandHandler(IProjectItemContentRepository content)
    {
        contentRepository = content;
    }
    public async Task<Resource<Unit>> Handle(CreateItemCommand command, CancellationToken cancellation)
    {
        await contentRepository.SaveProjectItemContent(
            await contentRepository
            .GetSystemProjectItemContent(
            Path.Combine(
                Constants.AppItemsTemplateFolder, $"{command.ItemName}{Constants.AppProjectTemplateExtension}")),
            command.ItemPath);

        return new Resource<Unit>.Success(Unit.Value);
    }
}
