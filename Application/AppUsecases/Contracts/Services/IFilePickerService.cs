using System.Threading.Tasks;

namespace AppUsecases.Contracts.Services;
public interface IFilePickerService
{
    public Task<string> GetFile(string[] fileFilters);
}
