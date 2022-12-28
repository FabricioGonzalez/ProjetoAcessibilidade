namespace AppUsecases.App.Contracts.Services;
public interface IFilePickerService
{
    public Task<string> GetFile(string[] fileFilters);
}
