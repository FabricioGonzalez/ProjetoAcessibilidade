namespace QuestPDFReport.Models;
public class ReportSectionCheckbox : ReportSectionElement
{
    public List<CheckboxModel> Checkboxes
    {
        get; set;
    } = new();
}
