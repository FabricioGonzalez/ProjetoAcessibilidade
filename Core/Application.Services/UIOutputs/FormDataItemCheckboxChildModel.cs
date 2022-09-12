namespace SystemApplication.Services.UIOutputs;

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