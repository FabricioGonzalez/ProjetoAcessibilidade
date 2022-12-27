﻿using Project.Application.Contracts;

using Splat;

namespace Project.Application.Implementations;
public class CommandDispatcher : ICommandDispatcher
{
    private readonly IReadonlyDependencyResolver _serviceProvider;

    public CommandDispatcher(IReadonlyDependencyResolver serviceProvider) => _serviceProvider = serviceProvider;
    public Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation)
    {
        var handler = _serviceProvider.GetService<ICommandHandler<TCommand, TCommandResult>>();
        return handler.Handle(command, cancellation);
    }
}
