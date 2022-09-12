namespace SystemApplication.Services.Contracts;
public interface ILastOpenRepository
{
    Task<IEnumerable<string>> GetRecentFiles();
}
