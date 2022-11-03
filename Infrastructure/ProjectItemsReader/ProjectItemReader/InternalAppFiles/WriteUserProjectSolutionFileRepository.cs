using System.Text.Json;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Project.Entities.Project;

namespace ProjectItemReader.InternalAppFiles;
public class WriteUserProjectSolutionFileRepository : IWriteContract<ProjectSolutionModel>
{
    public async Task<ProjectSolutionModel> WriteAsync(ProjectSolutionModel dataToWrite, string filePathToWrite)
    {
        var file = Directory.GetFiles(string.Join(Path.DirectorySeparatorChar
                , filePathToWrite.Split(Path.DirectorySeparatorChar)[..(filePathToWrite.Split(Path.DirectorySeparatorChar).Length - 1)]))
                .FirstOrDefault(file => file.Equals(filePathToWrite.Split(Path.DirectorySeparatorChar)[filePathToWrite.Split(Path.DirectorySeparatorChar).Length - 1]));

        if (file is not null)
        {
            using var writer = new StreamWriter(file);
            await writer.WriteAsync(JsonSerializer.Serialize(dataToWrite));

            /*await folder.CreateFolderAsync(Constants.USER_APP_PROJECT_ITEMS_FOLDER_NAME);*/

            return dataToWrite;
        }
        return null;

    }
}


