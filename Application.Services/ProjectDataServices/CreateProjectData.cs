using SystemApplication.Services.Contracts;

namespace SystemApplication.Services.ProjectDataServices;
public class CreateProjectData
{
    readonly IProjectSolutionRepository projectSolutionRepository;
    public CreateProjectData(IProjectSolutionRepository projectSolutionRepository)
    {
        this.projectSolutionRepository = projectSolutionRepository;
    }

    public async void CreateProjectItem(string projectPath, string ProjectItemName, string refPath)
    {
        await projectSolutionRepository.CreateProjectSolutionItem(projectPath, ProjectItemName, refPath);
    }
    public async void CreateProjectFolder(string projectPath, string ProjectItemName)
    {
        await projectSolutionRepository.CreateProjectSolutionFolder(projectPath, ProjectItemName);
    }
}
