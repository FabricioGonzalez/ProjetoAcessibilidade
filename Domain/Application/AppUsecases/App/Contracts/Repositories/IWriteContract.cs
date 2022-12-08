using System.Threading.Tasks;

namespace AppUsecases.Contracts.Repositories;
public interface IWriteContract<T>
{
    public Task WriteAsync(T dataToWrite, string filePathToWrite)
    {
        return default(Task);
    }
    public Task<T> WriteDataAsync(T dataToWrite, string filePathToWrite)
    {
        return default(Task<T>);
    }
}
