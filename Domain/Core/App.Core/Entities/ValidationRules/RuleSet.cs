namespace Core.Entities.ValidationRules;

public class RuleSet
{
    public string Operation
    {
        get; set;
    }
    public IEnumerable<Conditions> Conditions
    {
        get; set;
    }

}

public record Conditions(string TargetId,
    string Type,
    string CheckingValue,
    IEnumerable<string> Result,
    Func<string, (bool evaluationResult, IEnumerable<string> results)> ConditionsFunctions);