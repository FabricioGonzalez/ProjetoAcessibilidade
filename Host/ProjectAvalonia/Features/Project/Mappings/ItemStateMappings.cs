using System;
using System.IO;
using Common;
using ProjectAvalonia.Presentation.States.ProjectItems;
using XmlDatasource.ExplorerItems.Dto;

namespace ProjectAvalonia.Features.Project.Services;

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