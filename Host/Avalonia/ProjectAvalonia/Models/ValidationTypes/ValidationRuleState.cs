using System;
using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class ValidationRuleState
    : ReactiveObject
        , IValidationRuleState
{
    private ObservableCollection<IConditionState> _conditions = new();
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

    public ObservableCollection<IConditionState> Conditions
    {
        get => _conditions;
        set => this.RaiseAndSetIfChanged(ref _conditions, value);
    }
}

public class ConditionState
    : ReactiveObject
        , IConditionState
{
    private ICheckingOperationType _checkingOperationType;
    private ICheckingValue _checkingValue;
    private ObservableCollection<Result> _result = new();

    private string _targetId;

    public ConditionState()
    {
        this.WhenAnyValue(it => it.CheckingOperationType)
            .WhereNotNull()
            .Subscribe(val =>
            {
                ICheckingValue current = val switch
                {
                    IsOperation => new CheckedType(), _ => new TextType("")
                };

                CheckingValue = current;
            });
    }

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

    public ObservableCollection<Result> Result
    {
        get => _result;
        set => this.RaiseAndSetIfChanged(ref _result, value);
    }
}