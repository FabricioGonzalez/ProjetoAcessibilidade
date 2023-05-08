namespace ProjetoAcessibilidade.Domain.Contracts;

public interface ISender
{
    Task<TQueryResult> Send<TQueryResult>(
        IRequest<TQueryResult> request
        , CancellationToken cancellation
    );
}