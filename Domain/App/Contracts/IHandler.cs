namespace ProjetoAcessibilidade.Domain.Contracts;

public interface IHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(
        TRequest query
        , CancellationToken cancellation
    );
}