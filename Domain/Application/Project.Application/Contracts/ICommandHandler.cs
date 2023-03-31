namespace Project.Domain.Contracts;
public interface ICommandHandler<in TCommand, TCommandResult> where TCommand : IRequest<TCommandResult>
{
    Task<TCommandResult> Handle(TCommand command, CancellationToken cancellation);
}
