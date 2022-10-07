using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Entities.FileTemplate;

using Common;

using Windows.ApplicationModel;

namespace LocalRepository.FileRepository.Repository.InternalAppFiles;
public class ReadAllProjectTemplateFilesRepository : IReadContract<List<FileTemplate>>
{
    public async Task<List<FileTemplate>> ReadAsync()
    {
        var task = new Task<List<FileTemplate>>(() =>
        {
            var files = Directory.GetFiles(Path.Combine(
                Package.Current.InstalledPath, 
                Constants.ROOT_SYSTEM_PROJECT_TEMPLATE_FOLDER_NAME, 
                Constants.ROOT_APP_PROJECT_TEMPLATE_FOLDER_NAME));

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
    public async Task<List<FileTemplate>> ReadAsync(string path)
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
