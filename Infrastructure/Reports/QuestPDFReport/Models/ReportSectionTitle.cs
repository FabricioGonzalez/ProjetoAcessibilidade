namespace QuestPDFReport.Models;
public class ReportSectionTitle : ReportSectionElement
{

    public ReportSectionTitle(string Topic, string id)
    {
        Label = Topic;
        Id = id;
    }
}
