namespace Core.Entities.ValidationRules;

public interface IValidatableItem
{
    public string TargetIds
    {
        get; set;
    }

    public string Value
    {
        get; set;
    }
}