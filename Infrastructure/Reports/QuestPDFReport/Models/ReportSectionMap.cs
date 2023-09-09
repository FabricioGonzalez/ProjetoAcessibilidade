namespace QuestPDFReport.Models;

public class ReportSectionMap : ReportSectionElement
{
    public ReportSectionMap(
        string label
        , string id
    )
    {
        Label = label;
        Id = id;
    }

    public Location Location
    {
        get;
        set;
    }
}