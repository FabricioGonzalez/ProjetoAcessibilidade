using Projeto.Core.Models;

using SystemApplication.Services.Contracts;

namespace SystemApplication.Services.ProjectDataServices;
public class CreateProjectData
{
    readonly IProjectSolutionRepository projectSolutionRepository;
    public CreateProjectData(IProjectSolutionRepository projectSolutionRepository)
    {
        this.projectSolutionRepository = projectSolutionRepository;
    }

    public async Task<ProjectSolutionModel>? SaveProjectSolution(string solutionPath)
    {
        return await projectSolutionRepository.SaveProjectSolutionData(solutionPath);
    }

    public async void CreateProjectItem(string projectPath, string ProjectItemName, string refPath)
    {
        await projectSolutionRepository.CreateProjectSolutionItem(projectPath, ProjectItemName, refPath);
    }
    public async void RenameProjectFolder(string projectPath, string ProjectItemName)
    {
        await projectSolutionRepository.RenameProjectFolder(projectPath, ProjectItemName);
    }
    public async void RenameProjectItem(string projectPath, string ProjectItemName)
    {
        await projectSolutionRepository.RenameProjectItem(projectPath, ProjectItemName);
    }
}
