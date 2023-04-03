namespace QuestPDFReport.Models;
public class ReportSectionCheckbox : ReportSectionElement
{
    public ReportSectionCheckbox(string label, string id)
    {
        this.Label = label;
        this.Id = id;
    }
    public List<CheckboxModel> Checkboxes
    {
        get; set;
    } = new();
}
