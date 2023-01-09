using App.Core.Entities.Solution.Project.AppItem;

using Common;

using MediatR;

using Project.Application.Contracts;
using Project.Application.Project.Contracts;

namespace Project.Application.Project.Commands.ProjectItemCommands.SaveCommands;
public class SaveProjectItemContentCommand : IRequest<Resource<object>>
{
    public AppItemModel appItem;
    public string itemPath;
    public SaveProjectItemContentCommand(AppItemModel appItem, string itemPath)
    {
        this.appItem = appItem;
        this.itemPath = itemPath;
    }
}

public class SaveProjectItemContentCommandHandler : ICommandHandler<SaveProjectItemContentCommand, Resource<object>>
{
    private readonly IProjectItemContentRepository contentRepository;
    public SaveProjectItemContentCommandHandler(IProjectItemContentRepository content)
    {
        contentRepository = content;
    }

    public async Task<Resource<object>> Handle(SaveProjectItemContentCommand command, CancellationToken cancellation)
    {
        await contentRepository.SaveProjectItemContent(command.appItem, command.itemPath);

        return null;
    }
}

