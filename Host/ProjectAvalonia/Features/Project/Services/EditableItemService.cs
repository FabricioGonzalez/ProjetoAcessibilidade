using System;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;

using Common;

using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Presentation.States;

using XmlDatasource.ProjectItems;
using XmlDatasource.ProjectItems.DTO;

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
    )
    {
        try
        {
            _projectItemsDatasource.SaveContentItem(path: path
            , item: itemContent.ToItemRoot());

            NotificationHelpers.ShowMain("Salvo com sucesso", $"O item {itemContent.ItemName} foi salvo", 1, Avalonia.Controls.Notifications.NotificationType.Success);
        }
        catch (System.Exception ex)
        {
            Logger.LogError(ex);
            NotificationHelpers.ShowMain("Erro ao salvar", $"O item {itemContent.ItemName} não foi salvo", 1, Avalonia.Controls.Notifications.NotificationType.Error);

        }
    }


    public async Task SaveEditingItem(
        ItemRoot itemContent
        , string path
    )
    {
        try
        {
            _projectItemsDatasource.SaveContentItem(path: path
            , item: itemContent);

            NotificationHelpers.ShowMain("Salvo com sucesso", $"O item {itemContent.ItemName} foi salvo", 1, Avalonia.Controls.Notifications.NotificationType.Success);
        }
        catch (System.Exception ex)
        {
            Logger.LogError(ex);
            NotificationHelpers.ShowMain("Erro ao salvar", $"O item {itemContent.ItemName} não foi salvo", 1, Avalonia.Controls.Notifications.NotificationType.Error);

        }
    }

    public async Task CreateTemplateEditingItem(
        AppModelState itemContent
    )
    {
        try
        {
            _projectItemsDatasource.SaveContentItem(path: Path.Combine(path1: Constants.AppItemsTemplateFolder,
                path2: $"{itemContent.ItemTemplate}{Constants.AppProjectTemplateExtension}")
            , item: itemContent.ToItemRoot());
        }
        catch (System.Exception ex)
        {
            Logger.LogError(ex);
            NotificationHelpers.ShowMain("Erro ao salvar", $"O item {itemContent.ItemName} não foi salvo", 1, Avalonia.Controls.Notifications.NotificationType.Error);

        }

    }


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

    internal string GetConclusion(string itemPath)
    {
        return _projectItemsDatasource.GetConclusionItem(itemPath);
    }
}