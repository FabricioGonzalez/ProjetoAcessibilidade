using Common;
using Core.Entities.Solution.Explorer;
using Core.Entities.Solution.ItemsGroup;
using Project.Domain.App.Contracts;

namespace ProjectItemReader.InternalAppFiles;

public class AppTemplateRepositoryImpl : IAppTemplateRepository
{
    public async Task<Resource<List<ItemModel>>> ReadAllTemplateItems()
    {
        try
        {
            var task = new Task<Resource<List<ItemModel>>>(function: () =>
            {
                var files = Directory.GetFiles(path: Constants.AppItemsTemplateFolder);

                var filesList = new List<ItemModel>();

                foreach (var item in files)
                {
                    var splitedItem = item.Split(separator: Path.DirectorySeparatorChar);

                    if (Path.GetExtension(path: splitedItem.Last())
                        == Constants.AppProjectTemplateExtension)
                    {
                        filesList.Add(item: new ItemModel
                        {
                            Name = Path.GetFileNameWithoutExtension(path: splitedItem.Last()),
                            ItemPath = item
                        });
                    }
                }

                return filesList.Count > 0
                    ? new Resource<List<ItemModel>>.Success(Data: filesList)
                    : new Resource<List<ItemModel>>.Error(Message: ErrorConstants.AppNoFileFound, Data: null);
            });
            task.Start();

            return await task;
        }
        catch (Exception ex)
        {
            return new Resource<List<ItemModel>>.Error(Message: ex.Message, Data: null);
        }
    }
}