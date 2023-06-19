namespace Core.Entities.ValidationRules;
public class ValidationRule
{
    public IEnumerable<Targets> Targets
    {
        get; set;
    }

    public IEnumerable<RuleSet> Rules
    {
        get; set;
    }
}
