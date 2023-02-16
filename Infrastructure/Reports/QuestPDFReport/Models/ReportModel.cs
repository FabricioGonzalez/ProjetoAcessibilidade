namespace QuestPDFReport.Models;
public class ReportModel : IReport
{
    public string Title
    {
        get; set;
    }
    public byte[] LogoData
    {
        get; set;
    }
    public List<ReportHeaderField> HeaderFields
    {
        get; set;
    } = new();

    public List<ReportSection> Sections
    {
        get; set;
    } = new();
    public List<ReportPhoto> Photos
    {
        get; set;
    } = new();

}
public class NestedReportModel : IReport
{
    public string Title
    {
        get; set;
    }
    public byte[] LogoData
    {
        get; set;
    }
    public List<ReportHeaderField> HeaderFields
    {
        get; set;
    } = new();

    public List<ReportSectionGroup> Sections
    {
        get; set;
    } = new();
    public List<ReportPhoto> Photos
    {
        get; set;
    } = new();
}

public interface IReport
{
    public string Title
    {
        get; set;
    }
    public byte[] LogoData
    {
        get; set;
    }
    public List<ReportHeaderField> HeaderFields
    {
        get; set;
    }
}