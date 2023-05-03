using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;
using ProjetoAcessibilidade.Core.Entities.Solution.ReportInfo;

namespace ProjetoAcessibilidade.Core.Entities.Solution;

public class ProjectSolutionModel
{
    public SolutionInfo SolutionReportInfo
    {
        get;
        set;
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