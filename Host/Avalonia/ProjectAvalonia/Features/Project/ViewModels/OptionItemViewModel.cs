using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class OptionItemViewModel : ReactiveObject, IOptionViewModel
{
    public OptionItemViewModel(string value, bool isChecked)
    {
        Value = value;
        IsChecked = isChecked;
    }

    public string Value
    {
        get;
    }

    public bool IsChecked
    {
        get;
    }
}