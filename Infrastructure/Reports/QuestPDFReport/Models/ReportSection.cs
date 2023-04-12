namespace QuestPDFReport.Models;

public class ReportSection : IReportSection
{
    public List<ReportSectionElement> Parts
    {
        get;
        set;
    } = new();

    public string Id
    {
        get;
        set;
    } = Guid.NewGuid().ToString();

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

    public string Id
    {
        get;
        set;
    } = Guid.NewGuid().ToString();

    public string Title
    {
        get;
        set;
    }
}

public interface IReportSection
{
    public string Id
    {
        get;
        set;
    }

    public string Title
    {
        get;
        set;
    }
}