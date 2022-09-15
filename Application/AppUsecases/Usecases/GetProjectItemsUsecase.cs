using AppUsecases.Contracts.Usecases;
using Common;

namespace AppUsecases.Usecases;

public class GetProjectItemsUsecase : IUsecaseContract<object,object>
{
    public Resource<object> execute(object parameter = null)
    {
        return new Resource<object>.Success(new());
    }
}
