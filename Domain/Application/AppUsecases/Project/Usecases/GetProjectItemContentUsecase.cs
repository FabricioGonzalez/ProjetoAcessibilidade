using System.Threading.Tasks;

using AppUsecases.App.Contracts.Repositories;
using AppUsecases.App.Contracts.Usecases;
using AppUsecases.Project.Entities.Project;

using Common;

namespace AppUsecases.Project.Usecases;
public class GetProjectItemContentUsecase : IQueryUsecase<string, AppItemModel>

{
    IReadContract<AppItemModel> readProjectItemContent;

    public GetProjectItemContentUsecase(IReadContract<AppItemModel> readContract)
    {
        readProjectItemContent = readContract;
    }

    public Resource<AppItemModel> execute(string parameter)
    {
        var result = readProjectItemContent.ReadAsync(parameter).Result;

        if (result is not null)
        {
            return new Resource<AppItemModel>.Success(result);
        }
        return new Resource<AppItemModel>.Error("", null);
    }
    public async Task<Resource<AppItemModel>> executeAsync(string parameter)
    {
        var result = await readProjectItemContent.ReadAsync(parameter);

        if (result is not null)
        {
            return new Resource<AppItemModel>.Success(result);
        }
        return new Resource<AppItemModel>.Error("", null);
    }
}
