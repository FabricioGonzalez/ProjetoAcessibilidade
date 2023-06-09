using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;
using ProjetoAcessibilidade.Core.Entities.Solution.ReportInfo;

namespace ProjetoAcessibilidade.Core.Entities.Solution;

public class ProjectSolutionModel : AggregateRoot
{
    private readonly List<ItemGroupModel> _itemGroups;

    private ProjectSolutionModel(
        Guid id,
        SolutionInfo solutionReportInfo,
        string filePath
    ) : base(id)
    {
        SolutionReportInfo = solutionReportInfo;
        FileName = Path.GetFileNameWithoutExtension(filePath);
        FilePath = filePath;

        _itemGroups = new List<ItemGroupModel>();
    }


    public SolutionInfo SolutionReportInfo
    {
        get;
        init;
    }

    public IReadOnlyCollection<ItemGroupModel> ItemGroups => _itemGroups;

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

    public static ProjectSolutionModel Create(
        string solutionPath,
        SolutionInfo reportInfo
    ) =>
        new(
            id: Guid.NewGuid(),
            solutionReportInfo: new SolutionInfo(),
            filePath: "");

    public void AddItemToSolution(
        ItemGroupModel item
    ) =>
        _itemGroups.Add(item);
}