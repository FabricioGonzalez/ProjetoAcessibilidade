using AppUsecases.Contracts.Repositories;
using AppUsecases.Editing.Entities;

using Common;


namespace ProjectItemReader.InternalAppFiles;
public class ReadAppTemplatesRepository : IReadContract<Resource<List<FileTemplate>>>
{
    public async Task<Resource<List<FileTemplate>>> ReadAsync()
    {
        try
        {
            var task = new Task<Resource<List<FileTemplate>>>(() =>
            {
                var files = Directory.GetFiles(Path.Combine(
                    Constants.AppItemsTemplateFolder));

                var filesList = new List<FileTemplate>();

                foreach (var item in files)
                {
                    var splitedItem = item.Split(Path.DirectorySeparatorChar);

                    if (Path.GetExtension(splitedItem.Last())
                    == Constants.AppProjectTemplateExtension)
                        filesList.Add(new()
                        {
                            Name = Path.GetFileNameWithoutExtension(splitedItem.Last()),
                            FilePath = item
                        });
                }
                if (filesList.Count > 0)
                    return new Resource<List<FileTemplate>>.Success(filesList);

                return new Resource<List<FileTemplate>>.Error(ErrorConstants.AppNoFileFound, null);
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
                    Constants.AppTemplatesFolder,
                    Constants.AppItemsTemplateFolder));

                var filesList = new List<FileTemplate>();

                foreach (var item in files)
                {
                    var splitedItem = item.Split(Path.PathSeparator);

                    FileTemplate fileItem = new()
                    {
                        Name = Path.GetFileNameWithoutExtension(splitedItem.Last()),
                        FilePath = item
                    };

                    filesList.Add(fileItem);
                }
                if (filesList.Count > 0)
                    return new Resource<List<FileTemplate>>.Success(filesList);

                return new Resource<List<FileTemplate>>.Error(ErrorConstants.AppNoFileFound, null);
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
