namespace ProjetoAcessibilidade.Domain.Contracts;

public interface ICommandDispatcher
{
    Task<TCommandResult> Execute<TCommandResult>(
        IRequest<TCommandResult> command
        , CancellationToken cancellation
    );
}