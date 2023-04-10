namespace Core.Common;

public abstract class BaseEntity
{
    public readonly List<BaseEvent> _domainEvents = new();
}