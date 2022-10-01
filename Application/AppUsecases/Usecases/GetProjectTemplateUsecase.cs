using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Contracts.Usecases;

using Common;

using LocalRepository.FileRepository.Repository.InternalAppFiles;

namespace AppUsecases.Usecases;
public class GetProjectTemplateUsecase : IQueryUsecase<List<FileTemplate>>
{
    readonly IReadContract<List<FileTemplate>> readProjectItems;
    public GetProjectTemplateUsecase(IReadContract<List<FileTemplate>> readProjectItems)
    {
        this.readProjectItems = readProjectItems;
    }
    public Resource<List<FileTemplate>> execute()
    {
        var result = readProjectItems.ReadAsync().Result;
        if (result is not null)
        {
            return new Resource<List<FileTemplate>>.Success(result);
        }
        return new Resource<List<FileTemplate>>.Error("Algo deu errado",null);
    }
    public async Task<Resource<List<FileTemplate>>> executeAsync()
    {
        var result = await readProjectItems.ReadAsync();
        if (result is not null)
        {
            return new Resource<List<FileTemplate>>.Success(result);
        }
        return new Resource<List<FileTemplate>>.Error("Algo deu errado", null);
    }
}
