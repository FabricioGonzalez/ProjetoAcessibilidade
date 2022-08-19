using Core.Models;

namespace SystemApplication.Services.LastOpenModule.Contracts;
public interface ILastOpenService
{
    Task<LastOpenModel[]> GetLastOpenItems();
    Task<LastOpenModel> GetLastOpenItem(int index);
    void SetLastOpenItem(LastOpenModel item);

}
