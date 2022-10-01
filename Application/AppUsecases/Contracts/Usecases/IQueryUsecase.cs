using System.Threading.Tasks;

using Common;

namespace AppUsecases.Contracts.Usecases;
public interface IQueryUsecase<Input, Output>
{
    public Resource<Output> execute(Input parameter);
    public Task<Resource<Output>> executeAsync(Input parameter)
    {
        return default;
    }
}public interface IQueryUsecase<Output>
{
    public Resource<Output> execute();
    public Task<Resource<Output>> executeAsync()
    {
        return default;
    }
}
