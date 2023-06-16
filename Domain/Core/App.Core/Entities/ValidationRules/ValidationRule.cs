using System.Linq.Expressions;

namespace Core.Entities.ValidationRules;
public class ValidationRule
{
    public IEnumerable<Targets> Targets
    {
        get; set;
    }

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
