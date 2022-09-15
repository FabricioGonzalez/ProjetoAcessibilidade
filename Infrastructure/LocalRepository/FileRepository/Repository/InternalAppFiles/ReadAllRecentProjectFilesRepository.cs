using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AppUsecases.Contracts.Repositories;

using Windows.Storage;

namespace LocalRepository.FileRepository.Repository.InternalAppFiles;
public class ReadAllRecentProjectFilesRepository : IReadContract<List<FileTemplate>>
{
    public async Task<List<FileTemplate>> ReadFileAsync(string path)
    {
        var task = new Task<Task<List<FileTemplate>>>(async () =>
        {
            var recentFiles = new List<FileTemplate>();

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Recent);

            var folder = await StorageFolder.GetFolderFromPathAsync(path);

            var files = await folder.GetFilesAsync();

            foreach (var item in files)
            {
                if (item.IsOfType(StorageItemTypes.File))
                {
                    var splitedItem = item.Path.Split("\\");

                    recentFiles.Add(new()
                    {
                        Name = (splitedItem.GetValue(splitedItem.Length - 1) as string).Split(".")[0],
                        Path = item.Path
                    });
                }
            }

            folder = null;
            files = null;

            return recentFiles;
        });
        task.Start();

        return await await task;
    }
}
