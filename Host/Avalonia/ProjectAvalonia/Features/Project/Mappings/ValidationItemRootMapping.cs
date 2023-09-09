using System.Collections.ObjectModel;
using System.Linq;
using ProjectAvalonia.Models.ValidationTypes;
using ProjectAvalonia.Presentation.Interfaces;
using XmlDatasource.ValidationRules.DTO;

namespace ProjectAvalonia.Features.Project.Mappings;

public static class ValidationItemRootMapping
{
    public static ValidationItem ToValidationItem(
        this IValidationRuleContainerState state
    ) =>
        new()
        {
            Target = new RuleTargetsItem { Id = state.TargetContainerId, Name = state.TargetContainerName }
            , RuleConditions = state.ValidaitonRules.Select(it => new RuleConditionItem
            {
                RuleName = it.ValidationRuleName, Operation = it.Type.Value, RuleSetItems = it.Conditions.Select(
                    ruleSet => new RuleSetItem
                    {
                        ValueTrigger = new ValueTrigger
                        {
                            TargetValue = ruleSet.CheckingValue.Value, TargetId = ruleSet.TargetId
                            , Operation = ruleSet.CheckingOperationType.Value
                        }
                        , Results = ruleSet.Result.Select(result => new Results { Result = result.ResultValue })
                            .ToList()
                    }).ToList()
            }).ToList()
        };

    public static IValidationRuleContainerState ToValidationRuleContainerState(
        this ValidationItem root
    ) =>
        new ValidationRuleContainerState
        {
            TargetContainerId = root.Target.Id, TargetContainerName = root.Target.Name, ValidaitonRules =
                new ObservableCollection<IValidationRuleState>(root.RuleConditions.Select(y =>
                    new ValidationRuleState
                    {
                        ValidationRuleName = y.RuleName
                        , Type = AppValidation.GetOperationByValue(y.Operation)
                        , Conditions = new ObservableCollection<IConditionState>(y.RuleSetItems.Select(cond =>
                            new ConditionState
                            {
                                TargetId = cond.ValueTrigger.TargetId
                                , CheckingValue =
                                    AppValidation.GetCheckingOperationByValue(cond.ValueTrigger.Operation) is
                                        IsOperation
                                        ? AppValidation.GetCheckingValueByValue(cond.ValueTrigger.TargetValue)
                                        : new TextType(cond.ValueTrigger.TargetValue)
                                , CheckingOperationType =
                                    AppValidation.GetCheckingOperationByValue(cond.ValueTrigger.Operation)
                                , Result = new ObservableCollection<Result>(cond.Results.Select(it =>
                                    new Result { ResultValue = it.Result }))
                            }))
                    }))
        };
}