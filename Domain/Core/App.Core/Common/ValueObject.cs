namespace Core.Common;

public abstract class ValueObject
{
    protected static bool EqualOperator(
        ValueObject? left
        , ValueObject? right
    )
    {
        if (left is null ^ right is null)
        {
            return false;
        }

        return left?.Equals(obj: right!) != false;
    }

    protected static bool NotEqualOperator(
        ValueObject left
        , ValueObject right
    ) => !EqualOperator(left: left, right: right);

    protected abstract IEnumerable<object>? GetEqualityComponents();

    public override bool Equals(
        object? obj
    )
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;
        return GetEqualityComponents()?.SequenceEqual(second: other?.GetEqualityComponents()) ?? false;
    }

    public override int GetHashCode() =>
        GetEqualityComponents()
            ?.Select(selector: x => x is not null ? x.GetHashCode() : 0)
            ?.Aggregate(func: (
                x
                , y
            ) => x ^ y) ?? 0;
}