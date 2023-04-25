using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class CheckboxFormItem : ReactiveObject, ICheckboxFormItemViewModel
{
    public CheckboxFormItem(string topic, ObservableCollection<ICheckboxItemViewModel> checkboxItems)
    {
        Topic = topic;
        CheckboxItems = checkboxItems;
    }

    public string Topic
    {
        get;
    }

    public ObservableCollection<ICheckboxItemViewModel> CheckboxItems
    {
        get;
    }
}