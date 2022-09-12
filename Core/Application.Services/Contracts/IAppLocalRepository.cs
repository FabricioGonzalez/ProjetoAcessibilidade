using SystemApplication.Services.UIOutputs;

namespace SystemApplication.Services.Contracts;
public interface IAppLocalRepository
{
    List<FileTemplates> getProjectLocalPath();
    void GenerateHTML(List<ItemModel> items);
    Uri HTMLLinkGenerator();
}
