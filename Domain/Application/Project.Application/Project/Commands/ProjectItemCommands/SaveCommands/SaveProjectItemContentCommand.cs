using Common;

using Core.Entities.Solution.Project.AppItem;

using MediatR;

using Project.Application.Contracts;
using Project.Application.Project.Contracts;

namespace Project.Application.Project.Commands.ProjectItemCommands.SaveCommands;
public class SaveProjectItemContentCommand : IRequest<Resource<Unit>>
{
    public AppItemModel appItem;
    public string itemPath;
    public SaveProjectItemContentCommand(AppItemModel appItem, string itemPath)
    {
        this.appItem = appItem;
        this.itemPath = itemPath;
    }
}

public class SaveProjectItemContentCommandHandler : ICommandHandler<SaveProjectItemContentCommand, Resource<Unit>>
{
    private readonly IProjectItemContentRepository contentRepository;
    public SaveProjectItemContentCommandHandler(IProjectItemContentRepository content)
    {
        contentRepository = content;
    }

    public async Task<Resource<Unit>> Handle(SaveProjectItemContentCommand command, CancellationToken cancellation)
    {
        await contentRepository.SaveProjectItemContent(command.appItem, command.itemPath);

        return new Resource<Unit>.Success(Unit.Value);
    }
}

