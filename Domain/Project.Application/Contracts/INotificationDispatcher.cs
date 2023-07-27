namespace ProjetoAcessibilidade.Domain.Contracts;

public interface INotificationDispatcher
{
    Task Publish<TNotification>(
        TNotification notification
        , CancellationToken cancellation
    ) where TNotification : INotification;
}