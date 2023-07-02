using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IConditionState : INotifyPropertyChanged
{
    public ICheckingOperationType CheckingOperationType
    {
        get;
        set;
    }

    public string TargetId
    {
        get;
        set;
    }

    public ICheckingValue CheckingValue
    {
        get;
        set;
    }

    public ObservableCollection<string> Result
    {
        get;
        set;
    }
}