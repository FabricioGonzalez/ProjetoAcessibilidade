using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeCheckboxFormItem : ReactiveObject, ICheckboxFormItemViewModel
{
    public string Topic
    {
        get;
    } = "Teste";

    public ObservableCollection<ICheckboxItemViewModel> CheckboxItems
    {
        get;
    } = new()
    {
        new DesignTimeCheckboxItemViewModel(),
        new DesignTimeCheckboxItemViewModel(),
        new DesignTimeCheckboxItemViewModel(),
        new DesignTimeCheckboxItemViewModel()
    };
}