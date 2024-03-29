﻿/*using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DynamicData.Binding;
using ProjectAvalonia.Features.Project.States;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.ProjectItems;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;

namespace ProjectAvalonia.Common.Mappers;

public static class SolutionMapper
{
    public static ProjectSolutionModel ToSolutionModel(
        this SolutionState model
    ) =>
        new()
        {
            FileName = Path.GetFileNameWithoutExtension(model.FilePath), FilePath = model.FilePath, ItemGroups = model
                .ItemGroups
                .Select(selector: item => new ItemGroupModel
                {
                    Name = item.Name, ItemPath = item.ItemPath, Items = item
                        .Items
                        .Select(selector: child => new ItemModel
                        {
                            Id = child.Id, Name = child.Name, ItemPath = child.ItemPath,
                            TemplateName = child.TemplateName
                        }).ToList()
                })
                .ToList(),
            SolutionReportInfo = model.ReportData.ToReportData(),
            ParentFolderName = Directory.GetParent(path: model.FilePath).Name,
            ParentFolderPath = Directory.GetParent(path: model.FilePath).FullName
        };

    public static SolutionState ToSolutionState(
        this ProjectSolutionModel? model
    ) =>
        SolutionState.Create(filePath: model.FilePath, itemsGroups: new ObservableCollectionExtended<ItemGroupState>(
                collection: model
                    .ItemGroups
                    .Select(selector: item => new ItemGroupState
                    {
                        Name = item.Name, ItemPath = item.ItemPath, Items = new ObservableCollection<ItemState>(
                            collection: item
                                .Items
                                .Select(selector: child => new ItemState
                                {
                                    Id = child.Id, Name = child.Name, ItemPath = child.ItemPath,
                                    TemplateName = child.TemplateName
                                }))
                    })),
            reportData: model.SolutionReportInfo.ToReportState()
        );
}*/

