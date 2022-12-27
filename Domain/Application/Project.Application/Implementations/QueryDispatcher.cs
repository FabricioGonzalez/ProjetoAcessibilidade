using Project.Application.Contracts;

using Splat;

namespace Project.Application.Implementations;
public class QueryDispatcher : IQueryDispatcher
{
    private readonly IReadonlyDependencyResolver _serviceProvider;

    public QueryDispatcher(IReadonlyDependencyResolver serviceProvider) => _serviceProvider = serviceProvider;
    public Task<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query, CancellationToken cancellation)
    {
        var handler = _serviceProvider.GetService<IQueryHandler<TQuery, TQueryResult>>();
        return handler.Handle(query, cancellation);
    }
}
