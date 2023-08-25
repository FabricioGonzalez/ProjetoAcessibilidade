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
        set => this.RaiseAndSetIfChanged(backingField: ref _validationRuleName, newValue: value);
    }

    public IOperationType Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(backingField: ref _type, newValue: value);
    }

    public ObservableCollection<IConditionState> Conditions
    {
        get => _conditions;
        set => this.RaiseAndSetIfChanged(backingField: ref _conditions, newValue: value);
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
                var currentItemValue = _checkingValue?.Value ?? val.Value;

                if (val is IsOperation)
                {
                    CheckingValue = currentItemValue switch
                    {
                        "checked" => new CheckedType(), "unchecked" => new UnCheckedType()
                        , _ => new UnCheckedType()
                    };
                }
                else
                {
                    CheckingValue = new TextType(currentItemValue);
                    ;
                }
            });
    }

    public ICheckingOperationType CheckingOperationType
    {
        get => _checkingOperationType;
        set => this.RaiseAndSetIfChanged(backingField: ref _checkingOperationType, newValue: value);
    }

    public string TargetId
    {
        get => _targetId;
        set => this.RaiseAndSetIfChanged(backingField: ref _targetId, newValue: value);
    }

    public ICheckingValue CheckingValue
    {
        get => _checkingValue;
        set => this.RaiseAndSetIfChanged(backingField: ref _checkingValue, newValue: value);
    }

    public ObservableCollection<Result> Result
    {
        get => _result;
        set => this.RaiseAndSetIfChanged(backingField: ref _result, newValue: value);
    }
}