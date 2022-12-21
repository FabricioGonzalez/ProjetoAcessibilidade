using App.Core.Entities.Solution.Explorer;

using AppViewModels.Project.ComposableViewModels;

namespace AppViewModels.Project.Mappers;
public static class MapExplorerItem
{
    public static List<ProjectItemViewModel> GetSubfolders(this List<ExplorerItem> items)
    {
        List<ProjectItemViewModel> subfolders = new();

        if (items is not null && items.Count > 0)
        {

            foreach (var dir in items)
            {
                ProjectItemViewModel thisnode;

                if (dir is FolderItem item)
                {
                    thisnode = new FolderProjectItemViewModel(title: item.Name, path: item.Path, referencedItem: dir.ReferencedItem, inEditMode: false);

                    (thisnode as FolderProjectItemViewModel).Children = new(GetSubfolders(item.Children));
                }
                else
                {
                    thisnode = new FileProjectItemViewModel(title: dir.Name, path: dir.Path,referencedItem: dir.ReferencedItem, inEditMode: false);
                }

                subfolders.Add(thisnode);
            }
        }

        return subfolders;
    }

    public static FileProjectItemViewModel? SearchFile(this List<ProjectItemViewModel> items, ProjectItemViewModel desiredItem)
    {

        if (items is not null && items.Count > 0)
        {
            foreach (var dir in items)
            {
                if (dir is FileProjectItemViewModel file)
                {
                    if (file.Path.Equals(desiredItem.Path))
                    {
                        return file;
                    }
                }
                if (dir is FolderProjectItemViewModel folderItem)
                {
                    if (folderItem.Children.Any())
                       return folderItem.Children
                        .ToList()
                        .SearchFile(desiredItem);
                }
            }
        }
        return null;
    }

    public static FolderProjectItemViewModel? SearchFolder(this List<ProjectItemViewModel> items, ProjectItemViewModel desiredItem)
    {
        if (items is not null && items.Count > 0)
        {
            foreach (var dir in items)
            {
                if (dir is FolderProjectItemViewModel folder)
                {
                    if (folder.Path.Equals(desiredItem.Path))
                    {
                        return folder;
                    }
                    return folder.Children
                        .ToList()
                        .SearchFolder(desiredItem);
                }
            }
        }

        return null;
    }
}
