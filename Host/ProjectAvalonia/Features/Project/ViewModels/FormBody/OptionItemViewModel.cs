using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class OptionItemViewModel : ReactiveObject, IOptionViewModel
{
    public OptionItemViewModel(string value, bool isChecked, string id, bool isInvalid)
    {
        Value = value;
        IsChecked = isChecked;
        Id = id;
        IsInvalid = isInvalid;
    }

    public string Value
    {
        get;
    }
    [AutoNotify]
    private bool _isChecked = false;
    [AutoNotify]
    private bool _isInvalid = false;

    [AutoNotify]
    private bool _shouldBeUnchecked = false;
    public string Id
    {
        get;
        set;
    }
}