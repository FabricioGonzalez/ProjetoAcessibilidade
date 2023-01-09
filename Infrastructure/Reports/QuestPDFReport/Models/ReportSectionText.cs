namespace QuestPDFReport.Models;
public class ReportSectionText : ReportSectionElement
{
    public ReportSectionText(string text, string label)
    {
        Text = text;
        Label = label;
    }

    public string Text
    {
        get; set;
    }
}

