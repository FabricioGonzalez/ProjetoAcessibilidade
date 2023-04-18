using System.Reflection;
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
        var resu = ReflectionUtils.TypesImplementingInterface(
                desiredType: typeof(IQueryHandler<IRequest<TQueryResult>, TQueryResult>))
            .FirstOrDefault(predicate: t => t == typeof(IQueryHandler<TQuery, TQueryResult>));
        var handler = _serviceProvider.GetService<IQueryHandler<TQuery, TQueryResult>>();
        return handler.Handle(query: query, cancellation: cancellation);
    }
}

public static class ReflectionUtils
{
    public static bool DoesTypeSupportInterface(Type type, Type inter)
    {
        if (inter.IsAssignableFrom(c: type))
        {
            return true;
        }

        if (type.GetInterfaces().Any(predicate: i => i.IsGenericType && i.GetGenericTypeDefinition() == inter))
        {
            return true;
        }

        return false;
    }

    public static IEnumerable<Assembly> GetReferencingAssemblies(Assembly assembly) =>
        AppDomain
            .CurrentDomain
            .GetAssemblies().Where(predicate: asm => asm.GetReferencedAssemblies().Any(predicate: asmName =>
                AssemblyName.ReferenceMatchesDefinition(reference: asmName, definition: assembly.GetName())));

    public static IEnumerable<Type> TypesImplementingInterface(Type desiredType)
    {
        var assembliesToSearch = new[] { desiredType.Assembly }
            .Concat(second: GetReferencingAssemblies(assembly: desiredType.Assembly));
        return assembliesToSearch.SelectMany(selector: assembly => assembly.GetTypes())
            .Where(predicate: type => DoesTypeSupportInterface(type: type, inter: desiredType));
    }

    public static IEnumerable<Type> NonAbstractTypesImplementingInterface(Type desiredType) =>
        TypesImplementingInterface(desiredType: desiredType).Where(predicate: t => !t.IsAbstract);
}