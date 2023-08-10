namespace ProjetoAcessibilidade.Domain.Contracts;

public interface INotificationHandler
    <in TNotification>
    where TNotification : INotification
{
    Task HandleAsync(
        TNotification query
        , CancellationToken cancellation
    );
}