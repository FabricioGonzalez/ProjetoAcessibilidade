using System.Collections.Generic;
using System.Linq;
using ProjectAvalonia.Presentation.Enums;
using ProjectAvalonia.Presentation.Interfaces;

namespace ProjectAvalonia.Models.ValidationTypes;

public static class AppValidation
{
    public static readonly IEnumerable<ICheckingValue> checkingValues = new List<ICheckingValue>
    {
        new CheckedType(), new UnCheckedType()
    };

    public static readonly IEnumerable<IOperationType> operationTypes = new List<IOperationType>
    {
        new ObrigatorityOperation(), new ValueCheckOperation()
    };

    public static readonly IEnumerable<ICheckingOperationType> checkingOperationTypes = new List<ICheckingOperationType>
    {
        new HasOperation(), new GreaterThanOperation(), new LessThanOperation(), new GreaterOperation()
        , new LessOperation(), new IsOperation()
    };

    public static readonly IEnumerable<ICheckingRuleTypes> checkingRuleTypes = new List<ICheckingRuleTypes>
    {
        new CheckboxItemType(), new TextItemType()
    };


    public static ICheckingValue GetCheckingValueLocalized(
        string localized
    ) =>
        checkingValues.First(it => it.LocalizationKey == localized);


    public static IOperationType GetOperationLocalized(
        string localized
    ) =>
        operationTypes.First(it => it.LocalizationKey == localized);


    public static ICheckingOperationType GetCheckingOperationLocalized(
        string localized
    ) =>
        checkingOperationTypes.First(it => it.LocalizationKey == localized);

    public static ICheckingValue? GetCheckingValueByValue(
        string value
    ) =>
        checkingValues.FirstOrDefault(it => it.Value == value.TrimEnd());


    public static IOperationType? GetOperationByValue(
        string value
    ) =>
        operationTypes.FirstOrDefault(it => it.Value == value);


    public static ICheckingOperationType? GetCheckingOperationByValue(
        string value
    ) =>
        checkingOperationTypes.FirstOrDefault(it => it.Value == value);

    public static ICheckingRuleTypes? GetCheckingRuleTypeByValue(
        AppFormDataType value
    ) =>
        checkingRuleTypes.FirstOrDefault(it => it.Value == value);
}