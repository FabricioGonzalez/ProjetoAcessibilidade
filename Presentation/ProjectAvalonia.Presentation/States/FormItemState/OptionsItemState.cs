using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class OptionsItemState : ReactiveObject
{
    private string _id;

    private bool _isChecked;

    private string _value;

    public bool IsChecked
    {
        get => _isChecked;
        set => this.RaiseAndSetIfChanged(ref _isChecked, value);
    }

    public string Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }
}