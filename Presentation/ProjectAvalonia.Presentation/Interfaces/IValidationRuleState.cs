using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IValidationRuleState : INotifyPropertyChanged
{
    public string ValidationRuleName
    {
        get;
        set;
    }

    public IOperationType Type
    {
        get;
        set;
    }

    public ObservableCollection<IConditionState> Conditions
    {
        get;
        set;
    }
}