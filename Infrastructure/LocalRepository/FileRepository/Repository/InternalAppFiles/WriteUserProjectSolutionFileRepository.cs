using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Project.Entities.Project;
using Common;

using Windows.Storage;

namespace LocalRepository.FileRepository.Repository.InternalAppFiles;
public class WriteUserProjectSolutionFileRepository : IWriteContract<ProjectSolutionModel>
{
    public async Task<ProjectSolutionModel> WriteAsync(ProjectSolutionModel dataToWrite, string filePathToWrite)
    {
        var file = await StorageFile.GetFileFromPathAsync(filePathToWrite);
        if (file is not null)
        {
            var folder = await file.GetParentAsync();

            using var writer = await file.OpenStreamForWriteAsync();
            await writer.WriteAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(dataToWrite)));

           /* using var reader = new StreamReader(await file.OpenStreamForReadAsync());*/
            await folder.CreateFolderAsync(Constants.USER_APP_PROJECT_ITEMS_FOLDER_NAME);

            return dataToWrite;
        }
        return null;
    }
}

