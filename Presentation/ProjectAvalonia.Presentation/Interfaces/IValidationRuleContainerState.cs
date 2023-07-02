using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IValidationRuleContainerState : INotifyPropertyChanged
{
    public string TargetContainerId
    {
        get;
        set;
    }

    public string TargetContainerName
    {
        get;
        set;
    }

    public ObservableCollection<IValidationRuleState> ValidaitonRules
    {
        get;
        set;
    }
}