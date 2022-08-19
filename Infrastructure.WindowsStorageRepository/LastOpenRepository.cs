using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SystemApplication.Services.Contracts;

using Windows.Storage;

namespace Infrastructure.WindowsStorageRepository;
public class LastOpenRepository : ILastOpenRepository
{
    public async Task<List<string>> GetRecentFiles()
    {
        List<string> recentFiles = new List<string>();
        var path = Environment.GetFolderPath(Environment.SpecialFolder.Recent);

        var folder = await StorageFolder.GetFolderFromPathAsync(path);
       
        var files = await folder.GetFilesAsync();

        foreach (var item in files)
        {
            if (item.IsOfType(StorageItemTypes.File))
            {
                recentFiles.Add(item.Path);
            }
        }
        return recentFiles;
    }
}
