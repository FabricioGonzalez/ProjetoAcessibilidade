using App.Core.Entities.Solution.ItemsGroup;
using App.Core.Entities.Solution.ReportInfo;

namespace App.Core.Entities.Solution;
public class ProjectSolutionModel
{
    private SolutionInfo solutionReportInfo;
    public SolutionInfo SolutionReportInfo
    {
        get => solutionReportInfo;
        set => solutionReportInfo = value;
    }
    public List<ItemGroupModel> ItemGroups
    {
        get;
        set;
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
