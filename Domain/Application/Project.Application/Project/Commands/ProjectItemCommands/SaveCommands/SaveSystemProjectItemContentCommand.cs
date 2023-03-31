using Common;

using Core.Entities.Solution.Project.AppItem;

using Project.Domain.App.Models;
using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Commands.ProjectItemCommands.SaveCommands;
public sealed record SaveSystemProjectItemContentCommand(AppItemModel AppItem, string ItemPath) : IRequest<Resource<Empty>>;
public class SaveSystemProjectItemContentCommandHandler : ICommandHandler<SaveSystemProjectItemContentCommand, Resource<Empty>>
{
    private readonly IProjectItemContentRepository contentRepository;
    public SaveSystemProjectItemContentCommandHandler(IProjectItemContentRepository content)
    {
        contentRepository = content;
    }

    public async Task<Resource<Empty>> Handle(SaveSystemProjectItemContentCommand command, CancellationToken cancellation)
    {
        await contentRepository.SaveSystemProjectItemContent(command.AppItem, command.ItemPath);

        return new Resource<Empty>.Success(Empty.Value);
    }
}
