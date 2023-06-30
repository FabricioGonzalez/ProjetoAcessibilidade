using System.Collections.Generic;
using System.Linq;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.ValidationRulesState;

namespace ProjectAvalonia.Models.ValidationTypes;

public static class AppValidation
{
    public static readonly IEnumerable<ICheckingValue> checkingValues = new List<ICheckingValue>
    {
        new CheckedType(),
        new UnCheckedType()
    };

    public static readonly IEnumerable<IOperationType> operationTypes = new List<IOperationType>
    {
        new ObrigatorityOperation(),
        new ValueCheckOperation()
    };

    public static readonly IEnumerable<ICheckingOperationType> checkingOperationTypes = new List<ICheckingOperationType>
    {
        new HasOperation(),
        new GreaterThanOperation(),
        new LessThanOperation(),
        new GreaterOperation(),
        new LessOperation(),
        new IsOperation()
    };


    public static ICheckingValue GetCheckingValueLocalized(string localized) =>
        checkingValues.First(it => it.LocalizationKey == localized);


    public static IOperationType GetOperationLocalized(string localized) =>
        operationTypes.First(it => it.LocalizationKey == localized);


    public static ICheckingOperationType GetCheckingOperationLocalized(string localized) =>
        checkingOperationTypes.First(it => it.LocalizationKey == localized);

    public static ICheckingValue GetCheckingValueByValue(string value) =>
        checkingValues.First(it => it.Value == value);


    public static IOperationType GetOperationByValue(string value) =>
        operationTypes.First(it => it.Value == value);


    public static ICheckingOperationType GetCheckingOperationByValue(string value) =>
        checkingOperationTypes.First(it => it.Value == value);
}