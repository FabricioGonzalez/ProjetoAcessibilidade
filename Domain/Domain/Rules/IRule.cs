using Domain.Rules.FormRules;

namespace Domain.Rules;

public interface IRule
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