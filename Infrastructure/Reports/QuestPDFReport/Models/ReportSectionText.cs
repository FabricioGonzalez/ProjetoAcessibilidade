namespace QuestPDFReport.Models;

public class ReportSectionText : ReportSectionElement
{
    public ReportSectionText(
        string text
        , string label
        , string id
    )
    {
        Text = text;
        Label = label;
        Id = id;
    }

    public string Text
    {
        get;
        set;
    }
}