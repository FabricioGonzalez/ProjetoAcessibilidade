using ReactiveUI;

namespace ProjectAvalonia.Common.Models;

public class Printer : ReactiveObject
{
    private string _name = "";

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(backingField: ref _name, newValue: value);
    }
}