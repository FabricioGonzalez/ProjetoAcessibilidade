namespace Core.Models;

public class FormDataItemCheckboxChildModel
{
    public string Topic
    {
        get; set;
    }
    public List<OptionModel> Options
    {
        get; set;
    }
    public List<FormDataItemTextModel?> TextItems
    {
        get; set;
    }
}