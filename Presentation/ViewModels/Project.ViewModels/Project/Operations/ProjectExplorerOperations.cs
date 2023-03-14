using AppViewModels.Project.ComposableViewModels;
using AppViewModels.Project.Mappers;

using Common;

using Core.Entities.Solution.Explorer;

using Project.Domain.Contracts;
using Project.Domain.Project.Commands.ProjectItemCommands.DeleteCommands;
using Project.Domain.Project.Commands.ProjectItemCommands.RenameCommands;

using Splat;

namespace AppViewModels.Project.Operations;
public class ProjectExplorerOperations
{
    private readonly ICommandDispatcher commandDispatcher;

    public ProjectExplorerOperations()
    {
        commandDispatcher = Locator.Current.GetService<ICommandDispatcher>();
    }
    public async Task<FileProjectItemViewModel> RenameFile(FileProjectItemViewModel item, List<ProjectItemViewModel> items)
    {
        var file = items.SearchFile(desiredItem: item);

        if (file is not null)
        {
            var result = await commandDispatcher.Dispatch<RenameProjectFileItemCommand, Resource<ExplorerItem>>(new(
                 new()
                 {
                     Name = file.Title,
                     Path = file.Path,
                     ReferencedItem = file.ReferencedItem
                 }
                 ), CancellationToken.None);

            if (result is Resource<ExplorerItem>.Success operationResult)
            {
                return new(title: operationResult.Data.Name,
                    path: operationResult.Data.Path,
                    referencedItem: operationResult.Data.ReferencedItem,
                    inEditMode: false);

            }
            if (result is Resource<ExplorerItem>.Error)
            {

            }
            return item;
        }

        return item;
    }

    public async Task<FolderProjectItemViewModel> RenameFolder(FolderProjectItemViewModel item, List<ProjectItemViewModel> items)
    {
        var folder = items.SearchFolder(item);

        var result = await commandDispatcher.Dispatch<RenameProjectFolderItemCommand, Resource<ExplorerItem>>(new(
             new()
             {
                 Name = item.Title,
                 Path = item.Path,
                 Children = item.Children
                 .Select(x => new ExplorerItem()
                 {
                     Name = x.Title,
                     Path = x.Path,
                     ReferencedItem = x.ReferencedItem
                 })
                 .ToList()
             }
             ), CancellationToken.None);

        if (result is Resource<ExplorerItem>.Success operationResult)
        {
            return new(title: operationResult.Data.Name,
                path: operationResult.Data.Path,
                referencedItem: operationResult.Data.ReferencedItem,
                inEditMode: false);

        }
        if (result is Resource<ExplorerItem>.Error)
        {

        }
        return item;
    }

    public async Task<FileProjectItemViewModel> DeleteFile(FileProjectItemViewModel item, List<ProjectItemViewModel> items)
    {
        var file = items.SearchFile(item);

        if (file is not null)
        {
            var result = await commandDispatcher.Dispatch<DeleteProjectFileItemCommand, Resource<ExplorerItem>>(new(
                 new()
                 {
                     Name = file.Title,
                     Path = file.Path,
                     ReferencedItem = file.ReferencedItem,
                 }
                 ), CancellationToken.None);
            if (result is Resource<ExplorerItem>.Success operationResult)
            {
                return new(title: operationResult.Data.Name,
                    path: operationResult.Data.Path,
                    referencedItem: operationResult.Data.ReferencedItem,
                    inEditMode: false);
            }
            if (result is Resource<ExplorerItem>.Error)
            {

            }
        }

        return item;
    }

    public async Task<FolderProjectItemViewModel> DeleteFolder(FolderProjectItemViewModel item, List<ProjectItemViewModel> items)
    {
        var folder = items.SearchFolder(item);

        if (folder is not null)
        {
            var result = await commandDispatcher.Dispatch<DeleteProjectFolderItemCommand, Resource<ExplorerItem>>(new(
                 new()
                 {
                     Name = folder.Title,
                     Path = folder.Path,
                     Children = folder.Children
                     .Select(x => new ExplorerItem()
                     {
                         Name = x.Title,
                         Path = x.Path,
                         ReferencedItem = x.ReferencedItem,
                     })
                     .ToList()
                 }
                 ), CancellationToken.None);

            if (result is Resource<ExplorerItem>.Success operationResult)
            {
                return new(title: operationResult.Data.Name,
                    path: operationResult.Data.Path,
                    referencedItem: operationResult.Data.ReferencedItem,
                    inEditMode: false);

            }
            if (result is Resource<ExplorerItem>.Error)
            {

            }
        }

        return item;
    }

}
