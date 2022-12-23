using AppUsecases.App.Contracts.Repositories;
using AppUsecases.App.Contracts.Usecases;
using AppUsecases.Project.Entities.Project;

using Common;

namespace AppUsecases.Project.Usecases;
public class GetProjectSolutionUsecase : IQueryUsecase<string, ProjectSolutionModel>
{
    private readonly IReadContract<ProjectSolutionModel> readSolution;
    public GetProjectSolutionUsecase(IReadContract<ProjectSolutionModel> readSolution)
    {
        this.readSolution = readSolution;
    }
    public Resource<ProjectSolutionModel> execute(string path)
    {
        return null;
    }
    public async Task<Resource<ProjectSolutionModel>> executeAsync(string path)
    {
        var result = await readSolution.ReadAsync(path);

        if (result is null)
        {
            return new Resource<ProjectSolutionModel>.Error("Erro ao carregar solução", null);
        }

        return new Resource<ProjectSolutionModel>.Success(result);
    }
}
