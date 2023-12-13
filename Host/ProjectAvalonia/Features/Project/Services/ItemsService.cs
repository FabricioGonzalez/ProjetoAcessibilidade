using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Common;
using Common.Linq;
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.ProjectItems;
using XmlDatasource.ExplorerItems;
using XmlDatasource.ProjectItems;

namespace ProjectAvalonia.Features.Project.Services;

public sealed class ItemsService
{
    private readonly XmlExplorerItemDatasourceImpl _explorerItems;
    private readonly ProjectItemDatasourceImpl _projectItemDatasource;

    public ItemsService(
        XmlExplorerItemDatasourceImpl explorerItems
        , ProjectItemDatasourceImpl projectItemDatasource
    )
    {
        _explorerItems = explorerItems;
        _projectItemDatasource = projectItemDatasource;
    }

    public IEnumerable<ItemState> LoadAllItems() => _explorerItems.GetAllAppTemplates()
        .Match(s => s.Select(it => it.ToItemState()), f =>
        {
            Logger.LogError(f);
            return Enumerable.Empty<ItemState>();
        });

    public ExplorerEntry MoveItem(
        ExplorerEntry item
        , ExplorerEntry desiredItem
    )
    {
        try
        {
            if (File.GetAttributes(item.EntryPath) == FileAttributes.Directory && File.Exists(item.EntryPath))
            {
                Directory.Move(item.EntryPath, desiredItem.EntryPath);

                return desiredItem;
            }

            File.Move(item.EntryPath, desiredItem.EntryPath);

            return desiredItem;
        }
        catch (Exception e)
        {
            Logger.LogError(e);
            return item;
        }
    }

    public void SyncSolutionItems(
        ISolutionGroupViewModel solutionRootItem
    ) =>
        SyncLocationItems(solutionRootItem.LocationItems);

    private void SyncLocationItems(
        ObservableCollection<ISolutionLocationItem> solutionLocationItems
    ) =>
        solutionLocationItems.IterateOn(it =>
        {
            if (!Directory.Exists(it.ItemPath))
            {
                Directory.CreateDirectory(it.ItemPath.TrimEnd());
            }

            SyncItemsGroup(it.Items);
        });

    private void SyncItemsGroup(
        ObservableCollection<IItemGroupViewModel> itemGroupViewModels
    ) =>
        itemGroupViewModels.IterateOn(it =>
        {
            if (!Directory.Exists(it.ItemPath))
            {
                Directory.CreateDirectory(it.ItemPath.TrimEnd());
            }

            SyncItems(it.Items);
        });

    private void SyncItems(
        ObservableCollection<IItemViewModel> itemViewModels
    ) =>
        itemViewModels.IterateOn(async it =>
        {
            if (!File.Exists(it.ItemPath))
            {
                var item = await _projectItemDatasource.GetContentItem(Path.Combine(
                    Constants.AppItemsTemplateFolder
                    , $"{it.TemplateName}{Constants.AppProjectTemplateExtension}"));

                item.IfSucc(async fileResult =>
                {
                    _projectItemDatasource.SaveContentItem(it.ItemPath, fileResult);
                });
            }
        });

    public void ExcludeFolder(
        string argItemPath
    )
    {
        if (Directory.Exists(argItemPath))
        {
            Directory.Delete(argItemPath, true);
        }
    }

    public void ExcludeFile(
        string argItemPath
    )
    {
        if (File.Exists(argItemPath))
        {
            File.Delete(argItemPath);
        }
    }

    public void RenameFile(
        string oldPath
        , string newPath
    )
        {
        if (File.Exists(oldPath) && !File.Exists(newPath))
        {
            File.Move(oldPath, newPath);
        }
    }

    public void RenameFolder(
        string oldPath
        , string newPath
    )
    {
        if (Directory.Exists(oldPath) && !Directory.Exists(newPath))
        {
            Directory.Move(oldPath, newPath);
        }
    }
}

public class ExplorerEntry
{
    public string Name
    {
        get;
        set;
    }

    public string EntryPath
    {
        get;
        set;
    }
}