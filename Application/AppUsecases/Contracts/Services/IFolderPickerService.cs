using System.Threading.Tasks;

namespace AppUsecases.Contracts.Services;
public interface IFolderPickerService
{
    public Task<string> GetFolder();
}
