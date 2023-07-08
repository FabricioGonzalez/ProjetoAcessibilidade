using System.Xml.Serialization;
using Core.Entities.ValidationRules;
using ProjectItemReader.ValidationRulesExpression;

namespace ProjectItemReader.XmlFile.ValidationRules;

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

public static class ValidationRulesExtensions
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
                    return new RuleConditionItems
                    {
                        Operation = x.Operation, RuleSetItems = x.Conditions.Select(condition =>
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
                        res.target,
                        type,
                        value,
                        condition.Results.Select(result => result.Result),
                        item =>
                        {
                            var expression = _ruleLexer.MountEvaluation(item,
                                type,
                                value);

                            return (expression(), condition.Results.Select(results => results.Result));
                        });
                });

                var results = rule.RuleSetItems.SelectMany(results => results.Results.Select(y => y.Result));

                return new RuleSet
                {
                    Operation = rule.Operation, Conditions = conditions
                };
            })
        });
}