namespace QuestPDFReport.Models;

public class ReportSection : IReportSection
{
    public List<ReportSectionElement> Parts
    {
        get;
        set;
    } = new();

    public string Title
    {
        get;
        set;
    }
}

public class ReportSectionGroup : IReportSection
{
    public List<ReportSection> Parts
    {
        get;
        set;
    } = new();

    public string Title
    {
        get;
        set;
    }
}

public interface IReportSection
{
    public string Title
    {
        get;
        set;
    }
}