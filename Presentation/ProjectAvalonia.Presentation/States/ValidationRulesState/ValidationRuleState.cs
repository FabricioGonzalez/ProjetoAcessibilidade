using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.ValidationRulesState;

public class ValidationRuleState : ReactiveObject
{
    private ObservableCollection<ConditionState> _conditions = new();
    private IOperationType _type;
    private string _validationRuleName = "";


    public string ValidationRuleName
    {
        get => _validationRuleName;
        set => this.RaiseAndSetIfChanged(ref _validationRuleName, value);
    }

    public IOperationType Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(ref _type, value);
    }

    public ObservableCollection<ConditionState> Conditions
    {
        get => _conditions;
        set => this.RaiseAndSetIfChanged(ref _conditions, value);
    }
}

public class ConditionState : ReactiveObject
{
    private ICheckingOperationType _checkingOperationType;
    private ICheckingValue _checkingValue;
    private ObservableCollection<string> _result;

    private string _targetId;

    public ICheckingOperationType CheckingOperationType
    {
        get => _checkingOperationType;
        set => this.RaiseAndSetIfChanged(ref _checkingOperationType, value);
    }

    public string TargetId
    {
        get => _targetId;
        set => this.RaiseAndSetIfChanged(ref _targetId, value);
    }

    public ICheckingValue CheckingValue
    {
        get => _checkingValue;
        set => this.RaiseAndSetIfChanged(ref _checkingValue, value);
    }

    public ObservableCollection<string> Result
    {
        get => _result;
        set => this.RaiseAndSetIfChanged(ref _result, value);
    }
}

public interface ICheckingOperationType
{
    string Value
    {
        get;
    }

    string LocalizationKey
    {
        get;
    }
}