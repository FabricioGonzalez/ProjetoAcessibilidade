using Core.Entities.Solution;

namespace Project.Application.Solution.Contracts;
public interface ISolutionRepository
{
    public Task<ProjectSolutionModel> ReadSolution(string solutionPath);
    public Task SaveSolution(string solutionPath, ProjectSolutionModel dataToWrite);
    public Task SyncSolution(string solutionPath, ProjectSolutionModel dataToWrite);
}
