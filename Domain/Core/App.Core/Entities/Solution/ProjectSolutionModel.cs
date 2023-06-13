using System.Linq;

using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;
using ProjetoAcessibilidade.Core.Entities.Solution.ReportInfo;

namespace ProjetoAcessibilidade.Core.Entities.Solution;

public class ProjectSolutionModel : AggregateRoot
{
    private List<ItemGroupModel> _itemGroups;

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

    public void ReloadItem(List<ItemGroupModel> items)
    {
        _itemGroups = items;
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
            solutionReportInfo: reportInfo,
            filePath: solutionPath);

    public void AddItemToSolution(
        ItemGroupModel item
    ) =>
        _itemGroups.Add(item);
    public void RemoveFromSolution(
        Func<ItemGroupModel, bool> predicate
    ) =>
        _itemGroups.Remove(_itemGroups.First(predicate));
}