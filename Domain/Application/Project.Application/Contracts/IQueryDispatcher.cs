namespace ProjetoAcessibilidade.Domain.Contracts;

public interface IQueryDispatcher
{
    Task<TQueryResult> Dispatch<TQueryResult>(
        IRequest<TQueryResult> query
        , CancellationToken cancellation
    );
}