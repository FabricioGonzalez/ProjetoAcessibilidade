using Common;
using Core.Entities.Solution.Explorer;
using Project.Domain.App.Contracts;

namespace ProjectItemReader.InternalAppFiles;

public class AppTemplateRepositoryImpl : IAppTemplateRepository
{
    public async Task<Resource<List<ExplorerItem>>> ReadAllTemplateItems()
    {
        try
        {
            var task = new Task<Resource<List<ExplorerItem>>>(function: () =>
            {
                var files = Directory.GetFiles(path: Constants.AppItemsTemplateFolder);

                var filesList = new List<ExplorerItem>();

                foreach (var item in files)
                {
                    var splitedItem = item.Split(separator: Path.DirectorySeparatorChar);

                    if (Path.GetExtension(path: splitedItem.Last())
                        == Constants.AppProjectTemplateExtension)
                    {
                        filesList.Add(item: new ExplorerItem
                        {
                            Name = Path.GetFileNameWithoutExtension(path: splitedItem.Last()), Path = item
                        });
                    }
                }

                return filesList.Count > 0
                    ? new Resource<List<ExplorerItem>>.Success(Data: filesList)
                    : new Resource<List<ExplorerItem>>.Error(Message: ErrorConstants.AppNoFileFound, Data: null);
            });
            task.Start();

            return await task;
        }
        catch (Exception ex)
        {
            return new Resource<List<ExplorerItem>>.Error(Message: ex.Message, Data: null);
        }
    }
}