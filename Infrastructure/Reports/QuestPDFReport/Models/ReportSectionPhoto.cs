namespace QuestPDFReport.Models;
public class ReportSectionPhoto : ReportSectionElement
{
    public ReportSectionPhoto(string observation, string path, string id, string label = "")
    {
        Label = label;
        Observation = observation;
        Path = path;
        Id = id;
    }
    public string Observation
    {
        get; set;
    }
    public string Path
    {
        get; set;
    }
}
