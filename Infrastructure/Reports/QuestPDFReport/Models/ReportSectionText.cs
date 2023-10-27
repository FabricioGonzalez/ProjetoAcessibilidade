namespace QuestPDFReport.Models;

public class ReportSectionText : ReportSectionElement
{
    public ReportSectionText(
        string id
        , string text
        , string label
        , string measurementUnit)
    {
        Text = text;
        Label = label;
        Id = id;
        MeasurementUnit = measurementUnit;
    }

    public string Text
    {
        get;
        set;
    }

    public string MeasurementUnit
    {
        get;
        set;
    } = "";
}