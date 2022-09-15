using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppUsecases.Contracts.Repositories;

using Common;

namespace LocalRepository.FileRepository.Repository.InternalAppFiles;
public class ReadAllProjectTemplateFilesRepository : IReadContract<List<FileTemplate>>
{
    public async Task<List<FileTemplate>> ReadFileAsync(string path)
    {
        var task = new Task<List<FileTemplate>>(() =>
        {
            var files = Directory.GetFiles(Path.Combine(path, Constants.ROOT_APP_PROJECT_TEMPLATE_FOLDER_NAME));

            List<FileTemplate> filesList = new List<FileTemplate>();

            foreach (var item in files)
            {
                var splitedItem = item.Split("\\");
                filesList.Add(new()
                {
                    Name = (splitedItem.GetValue(splitedItem.Length - 1) as string).Split(".")[0],
                    Path = item
                });
            }
            return filesList;
        });
        task.Start();

        return await task;
    }
}
