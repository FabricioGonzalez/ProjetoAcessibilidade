using Project.Domain.Contracts;

using Splat;

namespace Project.Domain.Implementations;
public class CommandDispatcher : ICommandDispatcher
{
    private readonly IReadonlyDependencyResolver _serviceProvider;

    public CommandDispatcher(IReadonlyDependencyResolver serviceProvider) => _serviceProvider = serviceProvider;

    public Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation) where TCommand : IRequest<TCommandResult>
    {
        var handler = _serviceProvider.GetService<ICommandHandler<TCommand, TCommandResult>>();
        return handler.Handle((TCommand)command, cancellation);

    }
}


