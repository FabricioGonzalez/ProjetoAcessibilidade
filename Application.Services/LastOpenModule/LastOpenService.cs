using SystemApplication.Services.LastOpenModule.Contracts;

using Core.Models;

namespace SystemApplication.Services.LastOpenModule;

public class LastOpenService : ILastOpenService
{
    public LastOpenModel GetLastOpenItem(int index) {

        return new LastOpenModel();
    }
    public LastOpenModel[] GetLastOpenItems()
    {
        return new LastOpenModel[0];
    }
    public void SetLastOpenItem(LastOpenModel item)
    {
        
    }
}
