using System.Collections.Generic;
using System.Threading.Tasks;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.FileTemplate;

using Common;

namespace AppUsecases.Usecases;

public class GetProjectItemsUsecase : IQueryUsecase<string, List<ExplorerItem>>
{
    IReadContract<List<ExplorerItem>> readProjectItems;
    public GetProjectItemsUsecase(IReadContract<List<ExplorerItem>> readProjectItems)
    {
        this.readProjectItems = readProjectItems;
    }
    public Resource<List<ExplorerItem>> execute(string parameter)
    {
        //readProjectItems.ReadAsync()
        return new Resource<List<ExplorerItem>>.Success(new());
    }
    public async Task<Resource<List<ExplorerItem>>> executeAsync(string parameter)
    {
        var result = await readProjectItems.ReadAsync(parameter);

        if (result is null || result.Count == 0)
        {
            return new Resource<List<ExplorerItem>>.Error("Algo deu errado!!",null);
        }

        return new Resource<List<ExplorerItem>>.Success(result);
    }
}
