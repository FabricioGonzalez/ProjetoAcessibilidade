namespace QuestPDFTest.Models;
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
    }

    public List<ReportSection> Sections
    {
        get; set;
    }
    public List<ReportPhoto> Photos
    {
        get; set;
    }
}

