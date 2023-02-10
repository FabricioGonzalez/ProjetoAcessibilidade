namespace QuestPDFReport.Models;
public class ReportSection : IReportSection
{
    public string Title
    {
        get; set;
    }
    public List<ReportSectionElement> Parts
    {
        get; set;
    } = new();
}

public class ReportSectionGroup : IReportSection
{
    public string Title
    {
        get; set;
    }
    public List<ReportSection> Parts
    {
        get; set;
    } = new();
}

public interface IReportSection
{
    public string Title
    {
        get; set;
    }
}

