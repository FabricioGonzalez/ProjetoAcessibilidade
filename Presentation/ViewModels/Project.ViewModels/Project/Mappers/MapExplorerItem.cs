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
                    thisnode = new FolderProjectItemViewModel(title: item.Name, path: item.Path, inEditMode: false);

                    (thisnode as FolderProjectItemViewModel).Children = new(GetSubfolders(item.Children));
                }
                else
                {
                    thisnode = new FileProjectItemViewModel(title: dir.Name, path: dir.Path, inEditMode: false);
                }

                subfolders.Add(thisnode);
            }
        }

        return subfolders;
    }

    public static FileProjectItemViewModel? SearchFile(this List<ProjectItemViewModel> items, ProjectItemViewModel desiredItem)
    {
        FileProjectItemViewModel item = null;

        if (items is not null && items.Count > 0)
        {
            foreach (var dir in items)
            {
                if (dir is FileProjectItemViewModel file)
                {
                    if (file.Equals(desiredItem))
                    {
                        item = file;
                        return item;
                    }
                }
                if(dir is FolderProjectItemViewModel folderItem)
                {
                    folderItem.Children
                    .ToList()
                    .SearchFile(desiredItem);
                }                      
            }
        }

        return item;
    }

    public static FolderProjectItemViewModel? SearchFolder(this List<ProjectItemViewModel> items, ProjectItemViewModel desiredItem)
    {
        FolderProjectItemViewModel item = null;

        if (items is not null && items.Count > 0)
        {
            foreach (var dir in items)
            {
                if (dir is FolderProjectItemViewModel folder)
                {
                    if (folder.Equals(desiredItem))
                    {
                        item = folder;
                        return item;
                    }
                    folder.Children
                        .ToList()
                        .SearchFolder(desiredItem);
                }
            }
        }

        return item;
    }
}
