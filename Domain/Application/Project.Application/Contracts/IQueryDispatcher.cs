namespace Project.Domain.Contracts;

public interface IQueryDispatcher
{
    Task<TQueryResult> Dispatch<TQuery, TQueryResult>(
        TQuery query
        , CancellationToken cancellation
    )
        where TQuery : IRequest<TQueryResult>;
}