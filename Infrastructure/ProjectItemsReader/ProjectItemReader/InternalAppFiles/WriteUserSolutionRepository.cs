﻿using System.Text.Json;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Project.Entities.Project;

namespace ProjectItemReader.InternalAppFiles;
public class WriteUserSolutionRepository : IWriteContract<ProjectSolutionModel>
{
    public async Task<ProjectSolutionModel> WriteDataAsync(ProjectSolutionModel dataToWrite, string filePathToWrite)
    {
        var file = Directory.GetFiles(string.Join(Path.DirectorySeparatorChar
                , filePathToWrite.Split(Path.DirectorySeparatorChar)[..(filePathToWrite.Split(Path.DirectorySeparatorChar).Length - 1)]))
                .FirstOrDefault(file => file.Equals(filePathToWrite.Split(Path.DirectorySeparatorChar)[filePathToWrite.Split(Path.DirectorySeparatorChar).Length - 1]));

        StreamWriter writer = null; 

        if(file is null)
        {
            writer = new StreamWriter(File.Create(filePathToWrite));
        }

        if (file is not null)
        {
            writer = new StreamWriter(file);
            await writer.WriteAsync(JsonSerializer.Serialize(dataToWrite));

            /*await folder.CreateFolderAsync(Constants.USER_APP_PROJECT_ITEMS_FOLDER_NAME);*/

            writer.Close();
            await writer.DisposeAsync();

            return dataToWrite;
        }
        return null;

    }
}


