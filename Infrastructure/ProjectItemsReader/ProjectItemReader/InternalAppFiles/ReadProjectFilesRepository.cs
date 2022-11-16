using AppUsecases.Contracts.Repositories;
using AppUsecases.Project.Entities.FileTemplate;
using AppUsecases.Project.Enums;

using Common;

namespace ProjectItemReader.InternalAppFiles;
public class ReadProjectFilesRepository : IReadContract<List<ExplorerItem>>
{
    public async Task<List<ExplorerItem>> ReadAsync(string path)
    {
        var list = new List<ExplorerItem>();

        var splittedPath = path.Split(Path.DirectorySeparatorChar);

        var projectItemsRootPath = Path.Combine((string.Join(Path.DirectorySeparatorChar
            , splittedPath[..(splittedPath.Length - 1)])), Constants.AppUserProjectItemsFolder);

        if (Directory.Exists(projectItemsRootPath))
        {
            var folderItem = new FolderItem()
            {
                Name = projectItemsRootPath.Split(Path.DirectorySeparatorChar)[projectItemsRootPath.Split(Path.DirectorySeparatorChar).Length - 1],
                Path = projectItemsRootPath
            };

            folderItem.Children = new List<ExplorerItem>();

            list.Add(folderItem);

            var directory = Directory.GetFileSystemEntries(projectItemsRootPath);

            await GetDataFromPath(directory, folderItem.Children);
        }
        return list;
    }
    private async Task GetDataFromPath(string[] folder, IList<ExplorerItem> list)
    {
        foreach (var item in folder)
        {
            var fileAttributes = File.GetAttributes(item);

            if (File.GetAttributes(item).HasFlag(FileAttributes.Archive))
            {
                var i = new FileItem()
                {
                    Name = item.Split(Path.DirectorySeparatorChar)[item.Split(Path.DirectorySeparatorChar).Length - 1].Split(".")[0],
                    Path =item,
                };
                list.Add(i);
            }
            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                var itens = Directory.GetFiles(item);

                var folderItem = new FolderItem
                {
                    Name = item.Split(Path.DirectorySeparatorChar)[item.Split(Path.DirectorySeparatorChar).Length - 1],
                    Path = item,
                    Children = new List<ExplorerItem>()
                };

                foreach (var fileItem in itens)
                {
                    if (fileAttributes.HasFlag(FileAttributes.Directory))
                    {
                        var entries = Directory.GetFileSystemEntries(fileItem);

                        await GetDataFromPath(entries, folderItem.Children);
                    }
                    if (File.GetAttributes(item).HasFlag(FileAttributes.Archive))
                    {
                        var i = new FileItem()
                        {
                            Name = item.Split(Path.DirectorySeparatorChar)[item.Split(Path.DirectorySeparatorChar).Length - 1].Split(".")[0],
                            Path = item,
                        };
                        folderItem.Children.Add(i);
                    }
                }

                list.Add(folderItem);
            }

        }




    }
}
