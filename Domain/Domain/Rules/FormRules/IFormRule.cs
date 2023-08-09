using Domain.Rules.FormRules.Conditions;
using Domain.Shared.Enums;

namespace Domain.Rules.FormRules;

public interface IFormRule
{
    public RuleOperationType Operation
    {
        get;
        set;
    }

    public string RuleName
    {
        get;
        set;
    }

    public IEnumerable<ICondition> Conditions
    {
        get;
        set;
    }
}