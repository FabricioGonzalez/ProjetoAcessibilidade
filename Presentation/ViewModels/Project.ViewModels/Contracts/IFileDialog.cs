namespace AppViewModels.Contracts;
public interface IFileDialog
{
    public Task<string> GetFile();
    public Task<string> SaveFile();
    public Task<string> GetFolder();
}
