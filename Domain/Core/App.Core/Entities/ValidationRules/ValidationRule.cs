namespace Core.Entities.ValidationRules;

public class ValidationRule
{
    public Target Target
    {
        get;
        set;
    }

    public IEnumerable<RuleSet> Rules
    {
        get;
        set;
    }
}