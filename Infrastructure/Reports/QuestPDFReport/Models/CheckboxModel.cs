namespace QuestPDFReport.Models;

public class CheckboxModel
{
    public CheckboxModel(
        bool isChecked
        , string value
        , bool isValid = false
    )
    {
        IsChecked = isChecked;
        Value = value;
        IsValid = isValid;
    }

    public bool IsValid
    {
        get;
        set;
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