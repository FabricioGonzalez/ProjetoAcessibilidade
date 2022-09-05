using Projeto.Core.Models;

using SystemApplication.Services.Contracts;
using SystemApplication.Services.UIOutputs;

namespace SystemApplication.Services.ProjectDataServices;
public class CreateProjectData
{
    readonly IProjectSolutionRepository projectSolutionRepository;
    readonly IXmlProjectDataRepository xmlProjectDataRepository;
    public CreateProjectData(IProjectSolutionRepository projectSolutionRepository, IXmlProjectDataRepository xmlProjectDataRepository)
    {
        this.projectSolutionRepository = projectSolutionRepository;
        this.xmlProjectDataRepository = xmlProjectDataRepository;
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
    public async Task SaveProjectData(ItemModel model, string path)
    {
        await xmlProjectDataRepository.SaveModel(model, path);
    }
}
