using System.IO;
using System.Threading.Tasks;
using Common;
using ProjectAvalonia.Presentation.States;
using XmlDatasource.ProjectItems;

namespace ProjectAvalonia.Features.Project.Services;

public sealed class EditableItemService
{
    private readonly ProjectItemDatasourceImpl _projectItemsDatasource;

    public EditableItemService(
        ProjectItemDatasourceImpl projectItemsDatasource
    )
    {
        _projectItemsDatasource = projectItemsDatasource;
    }

    public async Task<AppModelState> GetEditingItem(
        string path
    ) =>
        (await _projectItemsDatasource.GetContentItem(path)).Match(Succ: succ => succ.ToAppModelState()
            , Fail: fail => new AppModelState());

    public void CreateEditingItem(
        AppModelState itemContent
    )
    {
        _projectItemsDatasource.SaveContentItem(Path.Combine(path1: Constants.AppItemsTemplateFolder,
            path2: $"{itemContent.ItemTemplate}{Constants.AppProjectTemplateExtension}"), itemContent.ToItemRoot());
    }
}