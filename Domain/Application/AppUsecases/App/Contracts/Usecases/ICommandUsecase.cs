using System.Threading.Tasks;

using Common;

namespace AppUsecases.App.Contracts.Usecases;
public interface ICommandUsecase<Input, Output>
{
    public Resource<Output> execute(Input parameter);
    public Task<Resource<Output>> executeAsync(Input parameter)
    {
        return default;
    }
}
public interface ICommandUsecase<Output>
{
    public Resource<Output> execute();
    public Task<Resource<Output>> executeAsync()
    {
        return default;
    }
}
