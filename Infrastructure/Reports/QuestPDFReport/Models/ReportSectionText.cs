namespace QuestPDFReport.Models;
public class ReportSectionText : ReportSectionElement
{

    public ReportSectionText(string text, string label, string id)
    {
        this.Text = text;
        this.Label = label;
        this.Id = id;

    }

    public string Text
    {
        get; set;
    }
}

