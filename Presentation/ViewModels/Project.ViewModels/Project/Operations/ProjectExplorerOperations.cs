using AppViewModels.Project.ComposableViewModels;
using AppViewModels.Project.Mappers;

using Project.Application.Project.Commands.ProjectItemCommands;

using Splat;

namespace AppViewModels.Project.Operations;
public class ProjectExplorerOperations
{
    private readonly RenameProjectFileItemCommandHandler renameProjectFileItemCommand;
    private readonly RenameProjectFolderItemCommandHandler renameProjectFolderItemCommand;
    private readonly DeleteProjectFileItemCommandHandler deleteProjectFileItemCommand;
    private readonly DeleteProjectFolderItemCommandHandler deleteProjectFolderItemCommand;

    public ProjectExplorerOperations()
    {
        renameProjectFileItemCommand = Locator.Current.GetService<RenameProjectFileItemCommandHandler>();
        renameProjectFolderItemCommand = Locator.Current.GetService<RenameProjectFolderItemCommandHandler>();
        deleteProjectFileItemCommand = Locator.Current.GetService<DeleteProjectFileItemCommandHandler>();
        deleteProjectFolderItemCommand = Locator.Current.GetService<DeleteProjectFolderItemCommandHandler>();
    }
    public async Task<FileProjectItemViewModel> RenameFile(FileProjectItemViewModel item, List<ProjectItemViewModel> items)
    {
        var file = items.SearchFile(desiredItem: item);

        if (file is not null)
        {
            var result = await renameProjectFileItemCommand.Handle(request: new(
                 new()
                 {
                     Name = file.Title,
                     Path = file.Path,
                     ReferencedItem= file.ReferencedItem
                 }
                 ), cancellationToken: CancellationToken.None);

            return new(title: result.Name, path: result.Path, referencedItem: result.ReferencedItem, inEditMode: false);
        }

        return item;
    }

    public async Task<FolderProjectItemViewModel> RenameFolder(FolderProjectItemViewModel item, List<ProjectItemViewModel> items)
    {
        var folder = items.SearchFolder(item);

        if (folder is not null)
        {
            var result = await renameProjectFolderItemCommand.Handle(request: new(
                 new()
                 {
                     Name = folder.Title,
                     Path = folder.Path,
                     Children = folder.Children
                     .Select(x => new App.Core.Entities.Solution.Explorer.ExplorerItem()
                     {
                         Name = x.Title,
                         Path = x.Path,
                         ReferencedItem = x.ReferencedItem
                     })
                     .ToList()
                 }
                 ), cancellationToken: CancellationToken.None);

            return new(title: result.Name, path: result.Path,referencedItem:result.ReferencedItem, inEditMode: false);
        }

        return item;
    }

    public async Task<FileProjectItemViewModel> DeleteFile(FileProjectItemViewModel item, List<ProjectItemViewModel> items)
    {
        var file = items.SearchFile(item);

        if (file is not null)
        {
            var result = await deleteProjectFileItemCommand.Handle(request: new(
                 new()
                 {
                     Name = file.Title,
                     Path = file.Path,
                     ReferencedItem = file.ReferencedItem,
                 }
                 ), cancellationToken: CancellationToken.None);

            return new(title: result.Name, path: result.Path,result.ReferencedItem, inEditMode: false);
        }

        return item;
    }

    public async Task<FolderProjectItemViewModel> DeleteFolder(FolderProjectItemViewModel item, List<ProjectItemViewModel> items)
    {
        var folder = items.SearchFolder(item);

        if (folder is not null)
        {
            var result = await deleteProjectFolderItemCommand.Handle(request: new(
                 new()
                 {
                     Name = folder.Title,
                     Path = folder.Path,
                     Children = folder.Children
                     .Select(x => new App.Core.Entities.Solution.Explorer.ExplorerItem()
                     {
                         Name = x.Title,
                         Path = x.Path,
                         ReferencedItem= x.ReferencedItem,
                     })
                     .ToList()
                 }
                 ), cancellationToken: CancellationToken.None);

            return new(title: result.Name, path: result.Path, referencedItem: result.ReferencedItem, inEditMode: false);
        }

        return item;
    }

}
