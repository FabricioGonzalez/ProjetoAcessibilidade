using AppUsecases.Contracts.Usecases;
using AppUsecases.Entities.FileTemplate;

using Common;

namespace AppUsecases.Usecases;

public class GetProjectItemsUsecase : IUsecaseContract<string,Resource<ExplorerItem>>
{
    public Resource<Resource<ExplorerItem>> execute(string parameter)
    {
        return new Resource<ExplorerItem>.Success(new());
    }
}
