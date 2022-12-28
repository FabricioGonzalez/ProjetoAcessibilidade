namespace AppUsecases.App.Contracts.Repositories;
public interface IWriteContract<T>
{
    public Task WriteAsync(T dataToWrite, string filePathToWrite)
    {
        return default;
    }
    public Task<T> WriteDataAsync(T dataToWrite, string filePathToWrite)
    {
        return default;
    }
}
