using AppUsecases.Contracts.Repositories;
using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.Project;

using Common;

namespace AppUsecases.Usecases;
public class CreateProjectSolutionUsecase : ICommandUsecase<ProjectSolutionModel, ProjectSolutionModel>
{
    private IWriteContract<ProjectSolutionModel> solutionCreator;

    public CreateProjectSolutionUsecase(IWriteContract<ProjectSolutionModel> solutionCreator)
    {
        this.solutionCreator = solutionCreator;
    }

    public Resource<ProjectSolutionModel> execute(ProjectSolutionModel model)
    {
        return default;
    }
    public async Task<Resource<ProjectSolutionModel>> executeAsync(ProjectSolutionModel model)
    {
        var result = await solutionCreator.WriteDataAsync(model,Path.Combine(model.FilePath,$"{model.FileName}{Constants.AppProjectSolutionExtension}"));

        if (result is null)
        {
            return new Resource<ProjectSolutionModel>.Error(Message: ErrorConstants.SolutionEmpty, null);
        }
        return new Resource<ProjectSolutionModel>.Success(Data: result);

    }
}
