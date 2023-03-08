using Common;

using Core.Entities.Solution.Project.AppItem;

using MediatR;

using Project.Application.Contracts;
using Project.Application.Project.Contracts;

namespace Project.Application.Project.Commands.ProjectItemCommands.SaveCommands;
public class SaveSystemProjectItemContentCommand : IRequest<Resource<Unit>>
{
    public AppItemModel appItem;
    public string itemPath;
    public SaveSystemProjectItemContentCommand(AppItemModel appItem, string itemPath)
    {
        this.appItem = appItem;
        this.itemPath = itemPath;
    }
}

public class SaveSystemProjectItemContentCommandHandler : ICommandHandler<SaveSystemProjectItemContentCommand, Resource<Unit>>
{
    private readonly IProjectItemContentRepository contentRepository;
    public SaveSystemProjectItemContentCommandHandler(IProjectItemContentRepository content)
    {
        contentRepository = content;
    }

    public async Task<Resource<Unit>> Handle(SaveSystemProjectItemContentCommand command, CancellationToken cancellation)
    {
        await contentRepository.SaveSystemProjectItemContent(command.appItem, command.itemPath);

        return new Resource<Unit>.Success(Unit.Value);
    }
}
