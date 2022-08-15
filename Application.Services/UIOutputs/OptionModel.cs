namespace SystemApplication.Services.UIOutputs;
public class OptionModel : NotifierBaseClass
{
    public string Value
    {
        get; set;
    }
    private bool isChecked;
    public bool IsChecked
    {
        get => isChecked;
        set => SetAtributeValue(ref isChecked, value);
    }
}