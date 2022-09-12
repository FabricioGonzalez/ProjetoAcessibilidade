using SystemApplication.Services.UIOutputs;

namespace SystemApplication.Services.Contracts;
public interface IXmlProjectDataRepository
{
    public Task<ItemModel> GetModel(string path);
    public Task SaveModel(ItemModel model, string path);
}
