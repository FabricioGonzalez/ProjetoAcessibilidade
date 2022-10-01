using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Contracts.Usecases;
using AppUsecases.Entities;

using Common;

namespace AppUsecases.Usecases;
public class GetProjectItemContentUsecase : IQueryUsecase<string,AppItemModel>

{
    IReadContract<AppItemModel> readProjectItemContent;

    public GetProjectItemContentUsecase(IReadContract<AppItemModel> readContract)
    {
        readProjectItemContent = readContract;
    }

    public Resource<AppItemModel> execute(string parameter)
    {
        var result = readProjectItemContent.ReadAsync(parameter).Result;

        if(result is not null)
        {
            return new Resource<AppItemModel>.Success(result);
        }
            return new Resource<AppItemModel>.Error("",null);
    }
}
