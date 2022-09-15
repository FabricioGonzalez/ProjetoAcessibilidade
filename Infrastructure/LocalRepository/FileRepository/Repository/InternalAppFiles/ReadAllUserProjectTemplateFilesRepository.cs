using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Entities.FileTemplate;

using Common;

using Windows.Storage;

namespace LocalRepository.FileRepository.Repository.InternalAppFiles;
public class ReadAllUserProjectTemplateFilesRepository : IReadContract<List<ExplorerItem>>
{
    public async Task<List<ExplorerItem>> ReadAsync(string path)
    {
        var list = new List<ExplorerItem>();

        var projectItemsRootPath = Path.Combine(path, Constants.USER_APP_PROJECT_ITEMS_FOLDER_NAME);

        if (Directory.Exists(projectItemsRootPath))
        {
            var directory = await StorageFolder.GetFolderFromPathAsync(projectItemsRootPath);

            await GetDataFromPath(directory, list);
        }
        return list;
    }
    private async Task GetDataFromPath(StorageFolder folder, IList<ExplorerItem> list)
    {
        var itens = await folder.GetItemsAsync();

        var folderItem = new ExplorerItem
        {
            Name = folder.Name,
            Path = folder.Path,
            Type = ExplorerItemType.Folder,
            Children = new List<ExplorerItem>()
        };

        foreach (var item in itens)
        {
            if (item.IsOfType(StorageItemTypes.Folder))
            {
                var newfolder = await StorageFolder.GetFolderFromPathAsync(item.Path);

                await GetDataFromPath(newfolder, folderItem.Children);
            }
            if (item.IsOfType(StorageItemTypes.File))
            {
                var i = new ExplorerItem()
                {
                    Name = item.Name.Split(".")[0],
                    Path = item.Path,
                    Type = ExplorerItemType.File
                };
                folderItem.Children.Add(i);
            }
        }

        list.Add(folderItem);
    }
}
