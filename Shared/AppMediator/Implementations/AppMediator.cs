using AppMediator.Contracts;
using Splat;

namespace AppMediator.Implementations;

public class AppMediator : IMediator
{
    private readonly IReadonlyDependencyResolver _serviceProvider;

    public AppMediator(
        IReadonlyDependencyResolver serviceProvider
    )
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResponse> Publish<TResponse>(INotification<TResponse> notification, CancellationToken cancellation)
        =>
            _serviceProvider.GetService<INotificationHandler<INotification<TResponse>, TResponse>>()
                .Handle(notification: notification, cancellation: cancellation);

    public Task Call<TCommand>(TCommand command, CancellationToken cancellation)
        =>
            _serviceProvider.GetService<ICommandHandler<TCommand>>()
                .Handle(command: command, cancellation: cancellation);

    Task<TQueryResult> IQueryDispatcher.Send<TQueryResult>(
        IRequest<TQueryResult> query
        , CancellationToken cancellation
    )
    {
        var handler = _serviceProvider.GetService<IQueryHandler<IRequest<TQueryResult>, TQueryResult>>();
        return handler.Handle(query: query, cancellation: cancellation);
    }
}