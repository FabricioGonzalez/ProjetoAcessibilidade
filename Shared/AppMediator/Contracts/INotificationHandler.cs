namespace AppMediator.Contracts;

public interface INotificationHandler<in TNotification, TResponse> where TNotification : INotification<TResponse>
{
    Task<TResponse> Handle(
        TNotification notification
        , CancellationToken cancellation
    );
}