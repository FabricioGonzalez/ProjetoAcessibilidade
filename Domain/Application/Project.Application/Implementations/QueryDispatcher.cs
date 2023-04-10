using Project.Domain.Contracts;
using Splat;

namespace Project.Domain.Implementations;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IReadonlyDependencyResolver _serviceProvider;

    public QueryDispatcher(
        IReadonlyDependencyResolver serviceProvider
    )
    {
        _serviceProvider = serviceProvider;
    }

    Task<TQueryResult> IQueryDispatcher.Dispatch<TQuery, TQueryResult>(
        TQuery query
        , CancellationToken cancellation
    )
    {
        var handler = _serviceProvider.GetService<IQueryHandler<TQuery, TQueryResult>>();
        return handler.Handle(query: query, cancellation: cancellation);
    }
}