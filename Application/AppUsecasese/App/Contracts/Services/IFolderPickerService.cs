using System.Threading.Tasks;

namespace AppUsecases.App.Contracts.Services;
public interface IFolderPickerService
{
    public Task<string> GetFolder();
}
