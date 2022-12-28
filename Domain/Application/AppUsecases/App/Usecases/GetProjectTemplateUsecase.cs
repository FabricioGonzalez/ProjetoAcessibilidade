using AppUsecases.App.Contracts.Repositories;
using AppUsecases.App.Contracts.Usecases;
using AppUsecases.Editing.Entities;

using Common;

namespace AppUsecases.App.Usecases;
public class GetProjectTemplateUsecase : IQueryUsecase<List<FileTemplate>>
{
    readonly IReadContract<Resource<List<FileTemplate>>> readProjectItems;
    public GetProjectTemplateUsecase(IReadContract<Resource<List<FileTemplate>>> readProjectItems)
    {
        this.readProjectItems = readProjectItems;
    }
    public Resource<List<FileTemplate>> execute()
    {
        var result = readProjectItems.ReadAsync().Result;
        if (result is not null)
        {
            return result switch
            {
                Resource<List<FileTemplate>>.Success item
                => new Resource<List<FileTemplate>>.Success(item.Data),

                Resource<List<FileTemplate>>.Error item
                => new Resource<List<FileTemplate>>.Error(item.Message, item.Data),

                Resource<List<FileTemplate>>.IsLoading item
                => new Resource<List<FileTemplate>>.IsLoading(item.Data, item.isLoading),
            };
        }
        return new Resource<List<FileTemplate>>.Error("Algo deu errado", null);
    }
    public async Task<Resource<List<FileTemplate>>> executeAsync()
    {
        var result = await readProjectItems.ReadAsync();
        if (result is not null)
        {
            return result switch
            {
                Resource<List<FileTemplate>>.Success item
                => new Resource<List<FileTemplate>>.Success(item.Data),

                Resource<List<FileTemplate>>.Error item
                => new Resource<List<FileTemplate>>.Error(item.Message, item.Data),

                Resource<List<FileTemplate>>.IsLoading item
                => new Resource<List<FileTemplate>>.IsLoading(item.Data, item.isLoading),
            };
        }
        return new Resource<List<FileTemplate>>.Error("Algo deu errado", null);
    }
}
