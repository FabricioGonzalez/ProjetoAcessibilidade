namespace Project.Domain.Contracts;
public interface ICommandDispatcher
{
    Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation) where TCommand : IRequest<TCommandResult>;
}
