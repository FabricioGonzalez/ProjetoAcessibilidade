using System.Threading.Tasks;

namespace AppUsecases.Contracts.Repositories;
public interface IReadContract<T>
{
    public Task<T> ReadAsync(string path);

}
