namespace ProjectAvalonia.Presentation.Interfaces;

public interface ICheckboxFormItemViewModel : IFormViewModel
{
    public string Topic
    {
        get;
    }

    public List<ICheckboxItemViewModel> CheckboxItems
    {
        get;
    }
}