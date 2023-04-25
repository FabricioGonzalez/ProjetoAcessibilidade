using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class CheckboxItemViewModel : ReactiveObject, ICheckboxItemViewModel
{
    public CheckboxItemViewModel(string topic, IOptionsContainerViewModel options,
        ObservableCollection<ITextFormItemViewModel> textItems)
    {
        Topic = topic;
        Options = options;
        TextItems = textItems;
    }

    public string Topic
    {
        get;
    }

    public IOptionsContainerViewModel Options
    {
        get;
    }

    public ObservableCollection<ITextFormItemViewModel> TextItems
    {
        get;
    }
}