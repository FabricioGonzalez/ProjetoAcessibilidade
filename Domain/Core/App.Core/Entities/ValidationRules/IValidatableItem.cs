namespace Core.Entities.ValidationRules;

public interface IValidatableItem
{
    public IEnumerable<string> TargetIds
    {
        get; set;
    }

    public IEnumerable<string> Values
    {
        get; set;
    }
}