using System.IO;
using System.Threading.Tasks;

using Common;

using ProjectAvalonia.Common.Logging;
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
            , Fail: fail =>
            {
                Logger.LogError(fail);

                return new AppModelState();
            });

    public async Task SaveEditingItem(
        AppModelState itemContent
        , string path
    ) =>
        (await _projectItemsDatasource.SaveContentItem(path: path
            , item: itemContent.ToItemRoot()))
        .IfFail(fail => Logger.LogError(fail));

    public async Task CreateTemplateEditingItem(
        AppModelState itemContent
    ) =>
        (await _projectItemsDatasource.SaveContentItem(path: Path.Combine(path1: Constants.AppItemsTemplateFolder,
                path2: $"{itemContent.ItemTemplate}{Constants.AppProjectTemplateExtension}")
            , item: itemContent.ToItemRoot()))
        .IfFail(fail => Logger.LogError(fail));


    public async Task SaveConclusionItem(
        string conclusionItemPath
        , string conclusionBody
    )
    {
        var result = await _projectItemsDatasource.SaveConclusionItem(conclusionItemPath: conclusionItemPath
                , conclusionBody: conclusionBody);

        result.IfFail(fail =>
        {
            Logger.LogError(fail);
        });
    }

}