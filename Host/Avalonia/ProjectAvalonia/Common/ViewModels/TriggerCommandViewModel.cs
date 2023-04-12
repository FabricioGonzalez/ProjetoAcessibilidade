using System.Windows.Input;
using ProjectAvalonia.ViewModels.Navigation;

namespace ProjectAvalonia.ViewModels;

public abstract class TriggerCommandViewModel : RoutableViewModel
{
    public abstract ICommand TargetCommand
    {
        get;
    }
}