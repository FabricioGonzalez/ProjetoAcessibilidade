using System.Collections.ObjectModel;

using SystemApplication.Services.UIOutputs;

namespace SystemApplication.Services.Contracts;
public interface IProjectSolutionRepository
{
    Task<List<FileTemplates>> getProjectLocalPath();
    Task<ObservableCollection<ExplorerItem>> GetData(string SolutionPath);
    Task CreateProjectSolutionItem(string projectPath, string ProjectItemName, string refPath);
    Task DeleteProjectSolutionItem(string projectPath);
    Task CreateProjectSolutionFolder(string projectPath, string ProjectFolder);
    Task DeleteProjectSolutionFolder(string projectPath);
}
