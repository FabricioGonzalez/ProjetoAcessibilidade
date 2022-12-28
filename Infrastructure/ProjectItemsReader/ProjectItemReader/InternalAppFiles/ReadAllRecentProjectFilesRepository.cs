/*using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AppUsecases.App.Contracts.Repositories;
using AppUsecases.Editing.Entities;

namespace ProjectItemReader.InternalAppFiles;
public class ReadAllRecentProjectFilesRepository : IReadContract<List<FileTemplate>>
{
    public async Task<List<FileTemplate>> ReadAsync(string path)
    {
        var task = new Task<Task<List<FileTemplate>>>(async () =>
        {
            var recentFiles = new List<FileTemplate>();

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Recent);

            var folder = Directory.GetDirectories(path).FirstOrDefault(folder => folder.Equals(path));

            var files = Directory.GetFileSystemEntries(path);

            foreach (var item in files)
            {
                if (File.GetAttributes(item).HasFlag(FileAttributes.Normal))
                {
                    var splitedItem = item.Split(Path.DirectorySeparatorChar);

                    recentFiles.Add(new()
                    {
                        Name = (splitedItem.GetValue(splitedItem.Length - 1) as string).Split(".")[0],
                        FilePath = item
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
*/