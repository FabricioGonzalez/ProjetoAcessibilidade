namespace QuestPDFReport.Models;

public class CheckboxModel
{
    public CheckboxModel(
        bool isChecked
        , string value
    )
    {
        IsChecked = isChecked;
        Value = value;
    }

    public bool IsChecked
    {
        get;
        set;
    }

    public string Value
    {
        get;
        set;
    }
}