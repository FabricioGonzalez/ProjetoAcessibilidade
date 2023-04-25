using System.Collections.ObjectModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ICheckboxFormItemViewModel : IFormViewModel
{
    public string Topic
    {
        get;
    }

    public ObservableCollection<ICheckboxItemViewModel> CheckboxItems
    {
        get;
    }
}