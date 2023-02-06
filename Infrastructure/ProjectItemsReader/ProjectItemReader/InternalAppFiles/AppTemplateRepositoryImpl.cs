using Common;

using Core.Entities.Solution.Explorer;

using Project.Application.App.Contracts;

namespace ProjectItemReader.publicAppFiles;

public class AppTemplateRepositoryImpl : IAppTemplateRepository
{
    public async Task<Resource<List<ExplorerItem>>> ReadAllTemplateItems()
    {
        try
        {
            var task = new Task<Resource<List<ExplorerItem>>>(() =>
            {
                var files = Directory.GetFiles(Path.Combine(
                    Constants.AppItemsTemplateFolder));

                var filesList = new List<ExplorerItem>();

                foreach (var item in files)
                {
                    var splitedItem = item.Split(Path.DirectorySeparatorChar);

                    if (Path.GetExtension(splitedItem.Last())
                    == Constants.AppProjectTemplateExtension)
                    {
                        filesList.Add(new()
                        {
                            Name = Path.GetFileNameWithoutExtension(splitedItem.Last()),
                            Path = item
                        });
                    }
                }
                return filesList.Count > 0
                    ? new Resource<List<ExplorerItem>>.Success(filesList)
                    : new Resource<List<ExplorerItem>>.Error(ErrorConstants.AppNoFileFound, null);
            });
            task.Start();

            return await task;
        }
        catch (Exception ex)
        {
            return new Resource<List<ExplorerItem>>.Error(ex.Message, null);
        }

    }
}
