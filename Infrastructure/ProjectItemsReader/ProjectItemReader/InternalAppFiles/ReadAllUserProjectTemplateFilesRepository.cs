using AppUsecases.Contracts.Repositories;
using AppUsecases.Project.Entities.FileTemplate;
using AppUsecases.Project.Enums;

using Common;

namespace ProjectItemReader.InternalAppFiles;
public class ReadAllUserProjectTemplateFilesRepository : IReadContract<List<ExplorerItem>>
{
    public async Task<List<ExplorerItem>> ReadAsync(string path)
    {
        var list = new List<ExplorerItem>();

        var projectItemsRootPath = Path.Combine(path, Constants.USER_APP_PROJECT_ITEMS_FOLDER_NAME);

        if (Directory.Exists(projectItemsRootPath))
        {
            var directory = Directory.GetFileSystemEntries(projectItemsRootPath);

            await GetDataFromPath(directory, list);
        }
        return list;
    }
    private async Task GetDataFromPath(string[] folder, IList<ExplorerItem> list)
    {
        foreach (var item in folder)
        {
            if(File.GetAttributes(item).HasFlag(FileAttributes.Directory))
            {
                var itens = Directory.GetFiles(item);

                var folderItem = new FolderItem
                {
                    Name = item.Split(Path.DirectorySeparatorChar)[item.Split(Path.DirectorySeparatorChar).Length -1],
                    Path = string.Join(Path.DirectorySeparatorChar, item.Split(Path.DirectorySeparatorChar)[..(item.Split(Path.DirectorySeparatorChar).Length - 1)]),
                    Children = new List<ExplorerItem>()
                };

                foreach (var fileItem in itens)
                {
                    if (File.GetAttributes(item).HasFlag(FileAttributes.Directory))
                    {
                        var entries = Directory.GetFileSystemEntries(fileItem);

                        await GetDataFromPath(entries, folderItem.Children);
                    }
                    if (File.GetAttributes(item).HasFlag(FileAttributes.Normal))
                    {
                        var i = new FileItem()
                        {
                            Name = item.Split(Path.DirectorySeparatorChar)[item.Split(Path.DirectorySeparatorChar).Length - 1].Split(".")[0],
                            Path = string.Join(Path.DirectorySeparatorChar, item.Split(Path.DirectorySeparatorChar)[..(item.Split(Path.DirectorySeparatorChar).Length - 1)]),
                        };
                        folderItem.Children.Add(i);
                    }
                }
               
                list.Add(folderItem);
            }

        }
       
       

     
    }
}
