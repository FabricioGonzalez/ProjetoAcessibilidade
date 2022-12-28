using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SystemApplication.Services.Contracts;

using Windows.Storage;

namespace Infrastructure.WindowsStorageRepository;
public class LastOpenRepository : ILastOpenRepository
{
    public async Task<IEnumerable<string>> GetRecentFiles()
    {
        List<string> recentFiles = new List<string>();
        var path = Environment.GetFolderPath(Environment.SpecialFolder.Recent);

        var folder = await StorageFolder.GetFolderFromPathAsync(path);

        var files =await folder.GetFilesAsync();

        return await Task.Run<IEnumerable<string>>(() => files.Select(x => x.IsOfType(StorageItemTypes.File) ? x.Path : ""));
        
    }
}
