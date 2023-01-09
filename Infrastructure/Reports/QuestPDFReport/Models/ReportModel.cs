namespace QuestPDFReport.Models;
public class ReportModel
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

