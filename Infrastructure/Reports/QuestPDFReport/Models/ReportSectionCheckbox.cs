namespace QuestPDFReport.Models;

public class ReportSectionCheckbox : ReportSectionElement
{
    public ReportSectionCheckbox(
        string label
        , string id
    )
    {
        Label = label;
        Id = id;
    }

    public List<CheckboxModel> Checkboxes
    {
        get;
        set;
    } = new();
}