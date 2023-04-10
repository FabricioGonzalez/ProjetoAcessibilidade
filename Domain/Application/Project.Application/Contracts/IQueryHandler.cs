namespace Project.Domain.Contracts;

public interface IQueryHandler<in TQuery, TQueryResult>
    where TQuery : IRequest<TQueryResult>
{
    Task<TQueryResult> Handle(
        TQuery query
        , CancellationToken cancellation
    );
}