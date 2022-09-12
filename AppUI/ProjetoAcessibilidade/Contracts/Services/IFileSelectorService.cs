using System.Threading.Tasks;

using Windows.Storage;

namespace ProjetoAcessibilidade.Contracts.Services;
public interface IFileSelectorService
{
    Task<StorageFile>  SaveFile(string filterName, string[] fileFilters, string fileName);
    Task<StorageFile>  OpenFile(string[] fileFilters);
}
