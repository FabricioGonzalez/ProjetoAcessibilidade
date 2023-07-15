namespace QuestPDFReport.Models;

public class ReportSection : IReportSection
{
    public List<ReportSectionElement> Parts
    {
        get;
        set;
    } = new();

    public IEnumerable<ImageSectionElement> Images
    {
        get;
        set;
    } = Enumerable.Empty<ImageSectionElement>();

    public IEnumerable<ObservationSectionElement> Observation
    {
        get;
        set;
    } = Enumerable.Empty<ObservationSectionElement>();

    public IEnumerable<LawSectionElement> Laws
    {
        get;
        set;
    } = Enumerable.Empty<LawSectionElement>();

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

public class ReportLocationGroup
{
    public List<ReportSectionGroup> Groups
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