namespace QuestPDFReport.Models;

public class ReportModel : IReport
{
    public List<ReportSection> Sections
    {
        get;
        set;
    } = new();

    public List<ReportPhoto> Photos
    {
        get;
        set;
    } = new();

    public string Title
    {
        get;
        set;
    }

    public string CompanyLogo
    {
        get;
        set;
    }

    public string StandardLaw
    {
        get;
        set;
    }

    public byte[] LogoData
    {
        get;
        set;
    }

    public List<ReportHeaderField> HeaderFields
    {
        get;
        set;
    } = new();
}

public class NestedReportModel : IReport
{
    public List<ReportSectionGroup> Sections
    {
        get;
        set;
    } = new();

    public List<ReportPhoto> Photos
    {
        get;
        set;
    } = new();

    public string Title
    {
        get;
        set;
    }

    public string CompanyLogo
    {
        get;
        set;
    }

    public string StandardLaw
    {
        get;
        set;
    }

    public byte[] LogoData
    {
        get;
        set;
    }

    public List<ReportHeaderField> HeaderFields
    {
        get;
        set;
    } = new();
}

public interface IReport
{
    public string Title
    {
        get;
        set;
    }
    public string CompanyLogo
    {
        get;
        set;
    }

    public string StandardLaw
    {
        get;
        set;
    }

    public byte[] LogoData
    {
        get;
        set;
    }

    public List<ReportHeaderField> HeaderFields
    {
        get;
        set;
    }
}