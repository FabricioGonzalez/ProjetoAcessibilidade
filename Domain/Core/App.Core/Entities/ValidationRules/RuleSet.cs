using System.Linq.Expressions;

namespace Core.Entities.ValidationRules;

public class RuleSet
{
    public string Operation
    {
        get; set;
    }

    public IEnumerable<Expression<Func<IValidatableItem,
        (bool evaluationResult, IEnumerable<string> results)>>> Conditions
    {
        get; set;
    }
}