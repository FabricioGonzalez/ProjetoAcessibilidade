namespace ProjetoAcessibilidade.Core.Common;

public abstract class BaseEntity
{
    public readonly List<BaseEvent> _domainEvents;

    public BaseEntity(
        Guid id
    )
    {
        _domainEvents = new List<BaseEvent>();
        Id = id;
    }

    public Guid Id
    {
        get;
        init;
    }

    public void RaiseDomainEvent(
        BaseEvent eventItem
    ) => _domainEvents?.Add(item: eventItem);

    public void RemoveDomainEvent(
        BaseEvent eventItem
    ) => _domainEvents?.Remove(item: eventItem);

    public void ClearDomainEvents() => _domainEvents?.Clear();
}