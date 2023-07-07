using System.Collections.ObjectModel;
using System.ComponentModel;
using ReactiveUI;

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

    public ObservableCollection<Result> Result
    {
        get;
        set;
    }
}

public class Result : ReactiveObject
{
    private string _resultValue = "";

    public string ResultValue
    {
        get => _resultValue;
        set => this.RaiseAndSetIfChanged(ref _resultValue, value);
    }
}