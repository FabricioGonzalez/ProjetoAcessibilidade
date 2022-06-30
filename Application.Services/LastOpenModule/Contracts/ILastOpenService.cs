using Core.Models;

namespace Application.Services.LastOpenModule.Contracts;
public interface ILastOpenService
{
    LastOpenModel[] GetLastOpenItems();
    LastOpenModel GetLastOpenItem(int index);
    void SetLastOpenItem(LastOpenModel item);

}
