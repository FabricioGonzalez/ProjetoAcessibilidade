namespace QuestPDFReport.Models;
public class ReportSection
{
    public string Title
    {
        get; set;
    }
    public List<ReportSectionElement> Parts
    {
        get; set;
    }
}

