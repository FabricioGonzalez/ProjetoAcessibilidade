using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;
using ProjetoAcessibilidade.Core.Entities.Solution.ReportInfo;

namespace ProjetoAcessibilidade.Core.Entities.Solution;

public class ProjectSolutionModel : AggregateRoot
{
    private List<SolutionGroupModel> _locationItems;

    private ProjectSolutionModel(
        Guid id
        , SolutionInfo solutionReportInfo
        , string filePath
    ) : base(id)
    {
        SolutionReportInfo = solutionReportInfo;
        FileName = Path.GetFileNameWithoutExtension(filePath);
        FilePath = filePath;

        _locationItems = new List<SolutionGroupModel>();
    }

    public SolutionInfo SolutionReportInfo
    {
        get;
        init;
    }

    public IReadOnlyCollection<SolutionGroupModel> LocationItems => _locationItems;

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

    public void ReloadItem(
        List<SolutionGroupModel> items
    ) => _locationItems = items;

    public static ProjectSolutionModel Create(
        string solutionPath
        , SolutionInfo reportInfo
    ) =>
        new(
            Guid.NewGuid(),
            reportInfo,
            solutionPath);

    public void AddItemToSolution(
        SolutionGroupModel item
    ) =>
        _locationItems.Add(item);

    public void RemoveFromSolution(
        Func<SolutionGroupModel, bool> predicate
    ) =>
        _locationItems.Remove(_locationItems.First(predicate));
}