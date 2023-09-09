namespace AppMediator.Contracts;

public interface INotificationDispatcher
{
    public Task<TResponse> Publish<TResponse>(INotification<TResponse> notification, CancellationToken cancellation);
}