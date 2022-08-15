using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemApplication.Services.UIOutputs;

namespace Infrastructure.InMemoryRepository;

public class GetAppLocal
{
    string path;
    public GetAppLocal(string path)
    {
        this.path = path;
    }
    public List<FileTemplates> getProjectLocalPath()
    {
        var files = Directory.GetFiles(Path.Combine(path, "Tables"));

        List<FileTemplates> filesList = new List<FileTemplates>();

        foreach (var item in files)
        {
            var splitedItem = item.Split("\\");
            filesList.Add(new()
            {
                Name = (splitedItem.GetValue(splitedItem.Length -1) as string).Split(".")[0],
                Path = item
            });
        }
        return filesList;
    }
}
