using ProjetoAcessibilidade.Domain.Contracts;

namespace ProjetoAcessibilidade.Domain.Implementations;

public class Mediator : IMediator
{
    private readonly IDictionary<Type, Type> _handlerDetails;
    private readonly IDictionary<Type, Type> _notificationHandlerDetails;
    private readonly Func<Type, object?> _serviceResolver;
    private readonly Func<Type, string?, object?> _serviceResolverWithContract;

    public Mediator(Func<Type, object?> serviceResolver, IDictionary<Type, Type> handlerDetails,
        IDictionary<Type, Type> notificationsDetails)
    {
        _serviceResolver = serviceResolver;
        _handlerDetails = handlerDetails;
        _notificationHandlerDetails = notificationsDetails;
    }

    public Mediator(Func<Type, string?, object?> serviceResolverWithContract, IDictionary<Type, Type> handlerDetails,
        IDictionary<Type, Type> notificationsDetails)
    {
        _serviceResolverWithContract = serviceResolverWithContract;
        _handlerDetails = handlerDetails;
        _notificationHandlerDetails = notificationsDetails;
    }

    public async Task<TCommandResult> Execute<TCommandResult>(IRequest<TCommandResult> command,
        CancellationToken cancellation)
    {
        var requestType = command.GetType();

        if (!_handlerDetails.ContainsKey(key: requestType))
        {
            throw new Exception(message: $"No handler to handle request of type:{requestType.Name}");
        }

        _handlerDetails.TryGetValue(key: requestType, value: out var requestHandler);

        var handler = _serviceResolver(arg: requestHandler);
        if (handler is null)
        {
            handler = _serviceResolverWithContract(arg1: requestHandler, arg2: null);
        }

        return await (Task<TCommandResult>)handler.GetType().GetMethod(name: "HandleAsync")
            .Invoke(obj: handler, parameters: new object?[] { command, cancellation });
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellation)
        where TNotification : INotification
    {
        var requestType = notification.GetType();

        if (!_notificationHandlerDetails.ContainsKey(key: requestType))
        {
            throw new Exception(message: $"No handler to handle request of type:{requestType.Name}");
        }

        _notificationHandlerDetails.TryGetValue(key: requestType, value: out var requestHandler);

        var handler = _serviceResolver(arg: requestHandler);
        if (handler is null)
        {
            handler = _serviceResolverWithContract(arg1: requestHandler, arg2: null);
        }

        await (Task)handler.GetType().GetMethod(name: "HandleAsync")
            .Invoke(obj: handler, parameters: new object?[] { notification, cancellation });
    }


    public async Task<TQueryResult> Dispatch<TQueryResult>(IRequest<TQueryResult> query, CancellationToken cancellation)
    {
        var requestType = query.GetType();

        if (!_handlerDetails.ContainsKey(key: requestType))
        {
            throw new Exception(message: $"No handler to handle request of type:{requestType.Name}");
        }

        _handlerDetails.TryGetValue(key: requestType, value: out var requestHandler);

        var handler = _serviceResolver(arg: requestHandler);
        if (handler is null)
        {
            handler = _serviceResolverWithContract(arg1: requestHandler, arg2: null);
        }

        return await (Task<TQueryResult>)handler.GetType().GetMethod(name: "HandleAsync")
            .Invoke(obj: handler, parameters: new object?[] { query, cancellation });
    }
}