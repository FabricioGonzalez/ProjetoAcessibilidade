using Domain.Rules.Enums;
using Domain.Rules.FormRules.Results;

namespace Domain.Rules.FormRules.Conditions;

public interface ICondition
{
    public RuleMatcherEnum Type
    {
        get;
        set;
    }

    public string TargetId
    {
        get;
        set;
    }

    public string CheckingValue
    {
        get;
        set;
    }

    public IEnumerable<Result> Results
    {
        get;
        set;
    }
}