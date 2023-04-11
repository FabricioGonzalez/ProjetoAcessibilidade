﻿using Common;
using Core.Entities.Solution.Project.AppItem;
using Project.Domain.App.Models;
using Project.Domain.Contracts;
using Project.Domain.Project.Contracts;

namespace Project.Domain.Project.Commands.ProjectItems;

public sealed record SaveProjectItemContentCommand(
    AppItemModel AppItem
    , string ItemPath
) : IRequest<Resource<Empty>>;

public sealed class SaveProjectItemContentCommandHandler
    : ICommandHandler<SaveProjectItemContentCommand, Resource<Empty>>
{
    private readonly IProjectItemContentRepository contentRepository;

    public SaveProjectItemContentCommandHandler(
        IProjectItemContentRepository content
    )
    {
        contentRepository = content;
    }

    public async Task<Resource<Empty>> Handle(
        SaveProjectItemContentCommand command
        , CancellationToken cancellation
    )
    {
        await contentRepository.SaveProjectItemContent(dataToWrite: command.AppItem, filePathToWrite: command.ItemPath);

        return new Resource<Empty>.Success(Data: Empty.Value);
    }
}