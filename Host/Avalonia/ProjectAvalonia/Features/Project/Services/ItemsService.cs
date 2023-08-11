using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using ProjectAvalonia.Presentation.States.ProjectItems;
using XmlDatasource.ExplorerItems;
using XmlDatasource.ExplorerItems.Dto;

namespace ProjectAvalonia.Features.Project.Services;

public sealed class ItemsService
{
    private readonly XmlExplorerItemDatasourceImpl _explorerItems;

    public ItemsService(
        XmlExplorerItemDatasourceImpl explorerItems
    )
    {
        _explorerItems = explorerItems;
    }

    public IEnumerable<ItemState> LoadAllItems() => _explorerItems.GetAllAppTemplates()
        .Match(Succ: s => s.Select(it => it.ToItemState()), Fail: f => Enumerable.Empty<ItemState>());
}

public static class ItemStateMappings
{
    public static ItemState ToItemState(
        this AppTemplateDto template
    ) =>
        new()
        {
            Name = template.Name, TemplateName = template.TemplateName, Id = Guid.NewGuid().ToString()
            , ItemPath = Path.Combine(path1: Constants.AppItemsTemplateFolder
                , path2: $"{template.TemplateName}{Constants.AppProjectTemplateExtension}")
        };
}