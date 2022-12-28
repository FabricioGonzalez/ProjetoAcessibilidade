namespace UIStatesStore.Project.Models;
public class ProjectModel
{
    private string _projectPath = "";
    public string ProjectPath
    {
        get => _projectPath;
        set => _projectPath = value;
    }

    public ProjectModel(string path)
    {
        ProjectPath = path;
    }
}
