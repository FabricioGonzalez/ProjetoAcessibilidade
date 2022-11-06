namespace AppUsecases.Project.Entities.Project;
public class ProjectSolutionModel
{
    public ReportDataModel reportData
    {
        get; set;
    }

    public string FileName
    {
        get;
        set;
    }
    public string FilePath
    {
        get;
        set;
    }
    public string ParentFolderName
    {
        get;
        set;
    }
    public string ParentFolderPath
    {
        get;
        set;
    }
}
