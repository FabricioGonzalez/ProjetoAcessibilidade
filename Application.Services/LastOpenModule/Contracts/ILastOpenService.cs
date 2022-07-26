using Core.Models;

namespace SystemApplication.Services.LastOpenModule.Contracts;
public interface ILastOpenService
{
    LastOpenModel[] GetLastOpenItems();
    LastOpenModel GetLastOpenItem(int index);
    void SetLastOpenItem(LastOpenModel item);

}
