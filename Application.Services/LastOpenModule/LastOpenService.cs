using SystemApplication.Services.LastOpenModule.Contracts;

using Core.Models;
using SystemApplication.Services.Contracts;

namespace SystemApplication.Services.LastOpenModule;

public class LastOpenService : ILastOpenService
{
    ILastOpenRepository _lastOpenRepository;
    public LastOpenService(ILastOpenRepository lastOpenRepository)
    {
        _lastOpenRepository = lastOpenRepository;
    }
    public async Task<LastOpenModel> GetLastOpenItem(int index)
    {
        var result = await _lastOpenRepository.GetRecentFiles();
        return new()
        {
            ProjectName = result.ElementAt(index).Split("\\")[result.ElementAt(index).Length - 1],
            ProjectPath = result.ElementAt(index)
        };
    }
    public async Task<LastOpenModel[]> GetLastOpenItems()
    {
        LastOpenModel[] items = new LastOpenModel[] { };

        var result = await _lastOpenRepository.GetRecentFiles();

        foreach (var item in result)
        {
            var e = new LastOpenModel();
            e.ProjectName = item.Split("\\")[item.Length - 1];
            e.ProjectPath = item;

            items.Append(e);
        }

        return items;
    }
    public void SetLastOpenItem(LastOpenModel item)
    {

    }
}
