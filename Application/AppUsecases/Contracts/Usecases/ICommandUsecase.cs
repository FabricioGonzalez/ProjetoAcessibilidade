using System.Threading.Tasks;

using Common;

namespace AppUsecases.Contracts.Usecases;
public interface ICommandUsecase<Input, Output>
{
    public Resource<Output> execute(Input parameter);
    public Task<Resource<Output>> executeAsync(Input parameter)
    {
        return default;
    }
}
