using AppUsecases.Contracts.Repositories;
using AppUsecases.Editing.Entities;

using Common;


namespace ProjectItemReader.InternalAppFiles;
public class ReadAllProjectTemplateFilesRepository : IReadContract<Resource<List<FileTemplate>>>
{
    public async Task<Resource<List<FileTemplate>>> ReadAsync()
    {
        try
        {
            var task = new Task<Resource<List<FileTemplate>>>(() =>
            {
                var files = Directory.GetFiles(Path.Combine(
                    Constants.ROOT_SYSTEM_PROJECT_TEMPLATE_FOLDER_NAME,
                    Constants.ROOT_APP_PROJECT_TEMPLATE_FOLDER_NAME));

                var filesList = new List<FileTemplate>();

                foreach (var item in files)
                {
                    var splitedItem = item.Split(Path.DirectorySeparatorChar);

                    if ((splitedItem.GetValue(splitedItem.Length - 1) as string).Split(".")[1] == "xml")
                        filesList.Add(new()
                        {
                            Name = (splitedItem.GetValue(splitedItem.Length - 1) as string).Split(".")[0],
                            Path = item
                        });
                }
                if (filesList.Count > 0)
                    return new Resource<List<FileTemplate>>.Success(filesList);

                return new Resource<List<FileTemplate>>.Error(ErrorConstants.APP_NO_FILE_ERROR, null);
            });
            task.Start();

            return await task;
        }
        catch (Exception ex)
        {
            return new Resource<List<FileTemplate>>.Error(ex.Message, null);
        }

    }
    public async Task<Resource<List<FileTemplate>>> ReadAsync(string path)
    {
        try
        {
            var task = new Task<Resource<List<FileTemplate>>>(() =>
            {
                var files = Directory.GetFiles(Path.Combine(
                    path,
                    Constants.ROOT_SYSTEM_PROJECT_TEMPLATE_FOLDER_NAME,
                    Constants.ROOT_APP_PROJECT_TEMPLATE_FOLDER_NAME));

                var filesList = new List<FileTemplate>();

                foreach (var item in files)
                {
                    var splitedItem = item.Split("\\");
                    filesList.Add(new()
                    {
                        Name = (splitedItem.GetValue(splitedItem.Length - 1) as string).Split(".")[0],
                        Path = item
                    });
                }
                if (filesList.Count > 0)
                    return new Resource<List<FileTemplate>>.Success(filesList);

                return new Resource<List<FileTemplate>>.Error(ErrorConstants.APP_NO_FILE_ERROR, null);
            });
            task.Start();

            return await task;
        }
        catch (Exception ex)
        {
            return new Resource<List<FileTemplate>>.Error(ex.Message, null);
        }
    }
}
