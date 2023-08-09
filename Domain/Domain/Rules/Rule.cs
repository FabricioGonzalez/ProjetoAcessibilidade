using Domain.Rules.FormRules;

namespace Domain.Rules;

public sealed class Rule : IRule
{
    public string ContainerId
    {
        get;
        set;
    }

    public IEnumerable<IFormRule> RuleDetails
    {
        get;
        set;
    }
}