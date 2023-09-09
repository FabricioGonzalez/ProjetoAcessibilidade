namespace QuestPDFReport.Models;

public class ReportSectionObservation : ReportSectionElement
{
    public ReportSectionObservation(
        string label
        , string id
        , string observation
    )
    {
        Label = label;
        Id = id;
        var Observation = observation;
    }

    public string Observation
    {
        get;
        set;
    }
}