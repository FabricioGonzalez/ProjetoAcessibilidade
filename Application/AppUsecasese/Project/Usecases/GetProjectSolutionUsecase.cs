using System.Threading.Tasks;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.Project;
using Common;

namespace AppUsecases.Usecases;
public class GetProjectSolutionUsecase : IQueryUsecase<ProjectSolutionModel>
{
    private readonly IReadContract<ProjectSolutionModel> readSolution;
    public GetProjectSolutionUsecase(IReadContract<ProjectSolutionModel> readSolution)
    {
        this.readSolution = readSolution;
    }
    public Resource<ProjectSolutionModel> execute()
    {
        return null;
    }    
    public async Task<Resource<ProjectSolutionModel>> executeAsync()
    {
        var result = await readSolution.ReadAsync();

        if (result is null)
            return new Resource<ProjectSolutionModel>.Error("Erro ao carregar solução",null);

        return new Resource<ProjectSolutionModel>.Success(result);
    }
}
