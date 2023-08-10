using System.Xml.Serialization;

namespace XmlDatasource.ValidationRules.DTO;

[XmlRoot("validation_rules")]
public class ValidationItemRoot
{
    [XmlElement("rule")]
    public List<ValidationItem> Rules
    {
        get;
        set;
    }
}

/*public static class ValidationRulesExtensions
{
    public static ValidationItemRoot MapToValidationRoot(
        this IEnumerable<ValidationRule> rules
        , RuleLexer _ruleLexer
    ) =>
        new()
        {
            Rules = rules.Select(x => new ValidationItem
            {
                Target = new RuleTargetsItem { Id = x.Target.Id }, RuleConditions = x.Rules.Select(x =>
                {
                    return new RuleConditionItem
                    {
                        RuleName = x.RuleName, Operation = x.Operation, RuleSetItems = x.Conditions.Select(condition =>
                        {
                            return new RuleSetItem
                            {
                                ValueTrigger = _ruleLexer.MountExpressionFrom(condition)
                                , Results = condition.Result.Select(result => new Results { Result = result }).ToList()
                            };
                        }).ToList()
                    };
                }).ToList()
            }).ToList()
        };

    public static IEnumerable<ValidationRule> MapToValidationRules(
        this ValidationItemRoot rules
        , RuleLexer _ruleLexer
    ) =>
        rules.Rules.Select(x => new ValidationRule
        {
            Target = new Target { Id = x.Target.Id }, Rules = x.RuleConditions.Select(rule =>
            {
                var conditions = rule.RuleSetItems.Select(condition =>
                {
                    var res = _ruleLexer.GetEvaluation(condition.ValueTrigger);

                    var type = res.evaluation.FirstOrDefault("is");
                    var value = res.evaluation.LastOrDefault("");
                    return new Conditions(
                        TargetId: res.target,
                        Type: type,
                        CheckingValue: value,
                        Result: condition.Results.Select(result => result.Result),
                        ConditionsFunctions: item =>
                        {
                            var expression = _ruleLexer.MountEvaluation(checkingValue: item,
                                evaluationType: type,
                                targetValue: value);

                            return (expression(), condition.Results.Select(results => results.Result));
                        });
                });

                return new RuleSet
                {
                    RuleName = rule.RuleName, Operation = rule.Operation, Conditions = conditions
                };
            })
        });
}*/