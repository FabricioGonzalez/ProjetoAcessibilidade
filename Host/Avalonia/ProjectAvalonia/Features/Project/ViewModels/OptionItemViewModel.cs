using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class OptionItemViewModel : ReactiveObject, IOptionViewModel
{
    public OptionItemViewModel(string value, bool isChecked, string id)
    {
        Value = value;
        IsChecked = isChecked;
        Id = id;
    }

    public string Value
    {
        get;
    }
    [AutoNotify]
    private bool _isChecked = false;
    public string Id
    {
        get;
        set;
    }
}