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
        .Match(Succ: s => s.Select(it => it.ToItemState()), Fail: f =>
        {
            Logger.LogError(f);
            return Enumerable.Empty<ItemState>();
        });

    /*public ItemState MoveItem(
        ItemState item
        , string desiredPath
    )
    {
        if (File.GetAttributes(item.ItemPath) == FileAttributes.Directory && File.Exists(item.ItemPath))
        {
            Directory.Move(sourceDirName: item.ItemPath, destDirName: desiredPath);

            item.ItemPath =
                Path.Combine(
                    path1: string.Join(separator: Path.DirectorySeparatorChar
                        , value: desiredPath.Split(Path.DirectorySeparatorChar)[..^1]), path2: item.Name);

            return item;
        }

        File.Move(sourceFileName: item.ItemPath, destFileName: desiredPath);

        item.ItemPath =
            Path.Combine(
                path1: string.Join(separator: Path.DirectorySeparatorChar
                    , value: desiredPath.Split(Path.DirectorySeparatorChar)[..^1])
                , path2: $"{item.Name}{Constants.AppProjectItemExtension}");

        return item;
    }
    */

    public ExplorerEntry MoveItem(
        ExplorerEntry item
        , ExplorerEntry desiredItem
    )
    {
        try
        {
            if (File.GetAttributes(item.EntryPath) == FileAttributes.Directory && File.Exists(item.EntryPath))
            {
                Directory.Move(sourceDirName: item.EntryPath, destDirName: desiredItem.EntryPath);

                return desiredItem;
            }

            File.Move(sourceFileName: item.EntryPath, destFileName: desiredItem.EntryPath);

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
                    path1: Constants.AppItemsTemplateFolder
                    , path2: $"{it.TemplateName}{Constants.AppProjectTemplateExtension}"));

                item.IfSucc(async fileResult =>
                {
                    await _projectItemDatasource.SaveContentItem(path: it.ItemPath, item: fileResult);
                });
            }
        });

    public void ExcludeFolder(
        string argItemPath
    )
    {
        if (Directory.Exists(argItemPath))
        {
            Directory.Delete(path: argItemPath, recursive: true);
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
        if (File.Exists(oldPath))
        {
            File.Move(sourceFileName: oldPath, destFileName: newPath);
            return;
        }

        File.Create(newPath);
    }

    public void RenameFolder(
        string oldPath
        , string newPath
    )
    {
        if (Directory.Exists(oldPath))
        {
            Directory.Move(sourceDirName: oldPath, destDirName: newPath);
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