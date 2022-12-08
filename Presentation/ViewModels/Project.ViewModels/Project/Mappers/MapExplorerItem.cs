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
                ProjectItemViewModel thisnode = new FileProjectItemViewModel()
                {
                    Title = dir.Name,
                    Path = dir.Path,
                    InEditMode = false,
                };

                if (dir is FolderItem item)
                {
                    thisnode = new FolderProjectItemViewModel()
                    {
                        Title = item.Name,
                        Path = item.Path,
                        InEditMode = false,
                    };

                    (thisnode as FolderProjectItemViewModel).Children = new(GetSubfolders(item.Children));
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
                (dir as FolderProjectItemViewModel)
                    .Children
                    .ToList()
                    .SearchFile(desiredItem);  
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
