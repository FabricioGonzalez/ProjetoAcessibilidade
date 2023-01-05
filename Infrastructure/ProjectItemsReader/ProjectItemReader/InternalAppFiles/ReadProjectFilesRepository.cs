/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppUsecases.App.Contracts.Repositories;
using AppUsecases.Project.Entities.FileTemplate;

using Common;

namespace ProjectItemReader.publicAppFiles;
public class ReadProjectFilesRepository : IReadContract<List<ExplorerItem>>
{
    public async Task<List<ExplorerItem>> ReadAsync(string path)
    {
        var list = new List<ExplorerItem>();

        var fileAttributes = File.GetAttributes(path);

        var projectItemsRootPath = "";

        if (fileAttributes.HasFlag(FileAttributes.Archive))
        {
            projectItemsRootPath = Path.Combine(Directory.GetParent(path).FullName, Constants.AppProjectItemsFolderName);
        }
        if (fileAttributes.HasFlag(FileAttributes.Directory))
        {
            Path.Combine(path, Constants.AppProjectItemsFolderName);
        }

        if (Directory.Exists(projectItemsRootPath))
        {
            var folderItem = new FolderItem
            {
                Name = Path.GetFileNameWithoutExtension(projectItemsRootPath),
                Path = projectItemsRootPath,
                Children = new List<ExplorerItem>()
            };

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
                    Name = Path.GetFileNameWithoutExtension(item),
                    Path = item,
                };
                list.Add(i);
            }

            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                var itens = Directory.GetFiles(item);

                var folderItem = new FolderItem
                {
                    Name = Path.GetFileNameWithoutExtension(item),
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
                            Name = Path.GetFileNameWithoutExtension(item),
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
*/