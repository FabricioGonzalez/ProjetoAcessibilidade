using System.Threading.Tasks;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Entities;

using Common;

namespace AppUsecases.Usecases;
internal class CreateProjectSolutionUsecase : ICommandUsecase<object, ProjectSolutionModel>
{
    public Resource<ProjectSolutionModel> execute(object parameter = null)
    {
        return default;
    }
/*    public async Task<Resource<ProjectSolutionModel>> executeAsync(object parameter = null)
    {

    }*/
}
