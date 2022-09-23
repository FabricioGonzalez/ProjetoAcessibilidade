using ReactiveUI;

namespace AppWinui.AppCode.Project.States.ProjectItems.Checkbox;

public class AppOptionModel : ReactiveObject
{
    public string Value
    {
        get; set;
    }
    private bool _isChecked = false;
    public bool IsChecked
    {
        get => _isChecked; 
        set => this.RaiseAndSetIfChanged(
            ref _isChecked, 
            value, 
            nameof(IsChecked));
    }
}