namespace AppUsecases.App.Contracts.Repositories;
public interface IReadContract<T>
{
    public Task<T> ReadAsync(string path)
    {
        return default;
    }
    public Task<T> ReadAsync()
    {
        return default;
    }

}
