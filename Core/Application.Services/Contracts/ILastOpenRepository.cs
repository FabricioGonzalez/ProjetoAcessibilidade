namespace SystemApplication.Services.Contracts;
public interface ILastOpenRepository
{
    Task<List<string>> GetRecentFiles();
}
