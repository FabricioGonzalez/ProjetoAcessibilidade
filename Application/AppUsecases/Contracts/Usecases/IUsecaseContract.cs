using Common;

namespace AppUsecases.Contracts.Usecases;
public interface IUsecaseContract<Input, Output>
{
    public Resource<Output> execute(Input? parameter);
}
