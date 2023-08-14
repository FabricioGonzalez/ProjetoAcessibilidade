using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class OptionsItemState
    : ReactiveObject
{
    private string _id;

    private bool _isChecked;

    private string _value;

    public bool IsChecked
    {
        get => _isChecked;
        set => this.RaiseAndSetIfChanged(backingField: ref _isChecked, newValue: value);
    }

    public string Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(backingField: ref _value, newValue: value);
    }

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(backingField: ref _id, newValue: value);
    }

    public bool IsInvalid
    {
        get;
        set;
    }
}