namespace AppRepositories.Solution.Dto;

public sealed class SolutionItem
{
    public ReportItem Report
    {
        get;
        set;
    }

    public IEnumerable<ProjectItem> ProjectItems
    {
        get;
        set;
    }
}