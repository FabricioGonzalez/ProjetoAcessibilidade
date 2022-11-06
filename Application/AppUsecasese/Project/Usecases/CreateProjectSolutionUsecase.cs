using System.Threading.Tasks;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.Project;
using Common;

namespace AppUsecases.Usecases;
public class CreateProjectSolutionUsecase : ICommandUsecase<ProjectSolutionModel>
{
    public Resource<ProjectSolutionModel> execute()
    {
        return default;
    }
/*    public async Task<Resource<ProjectSolutionModel>> executeAsync()
    {

    }*/
}
