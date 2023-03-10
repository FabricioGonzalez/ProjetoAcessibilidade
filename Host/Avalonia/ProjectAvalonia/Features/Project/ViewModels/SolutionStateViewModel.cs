﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Avalonia.Threading;

using Core.Entities.App;
using Core.Entities.Solution;
using Core.Entities.Solution.ItemsGroup;
using Core.Entities.Solution.ReportInfo;

using DynamicData.Binding;

using Project.Application.App.Queries.GetUFList;
using Project.Application.Contracts;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.Logging;
using ProjectAvalonia.ViewModels.Navigation;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

[NavigationMetaData(Title = "Success")]
public partial class SolutionStateViewModel : RoutableViewModel
{

    [AutoNotify] private SolutionInfo _reportData = new();

    [AutoNotify] private string _fileName = "";

    [AutoNotify] private string _filePath = "";

    [AutoNotify] private string _parentFolderName = "";

    [AutoNotify] private string _parentFolderPath = "";


    [AutoNotify] private ObservableCollectionExtended<ItemGroupState> _itemGroups = new();

    [AutoNotify] private ObservableCollectionExtended<UFModel> _ufList;

    private readonly IQueryDispatcher queryDispatcher;

    public SolutionStateViewModel()
    {
        queryDispatcher = Locator.Current.GetService<IQueryDispatcher>();

        ChooseSolutionPath = ReactiveCommand.CreateFromTask(async () =>
        {
            var path = await FileDialogHelper.ShowOpenFolderDialogAsync("Local da Solução");

            Dispatcher.UIThread.Post(() =>
            {
                FilePath = path;
                FileName = Path.GetFileNameWithoutExtension(path);
            });
        });

        ChooseLogoPath = ReactiveCommand.CreateFromTask(async () =>
        {
            var path = await FileDialogHelper.ShowOpenFileDialogAsync("Logo da Empresa", new string[] { "png" });

            Dispatcher.UIThread.Post(() =>
            {
                ReportData.LogoPath = path;
            });
        });

        ExcludeFileCommand = ReactiveCommand.Create<ItemState>((model) =>
        {
            Logger.LogInfo(model.Name);
        });

        RenameFileCommand = ReactiveCommand.Create<ItemState>((model) =>
        {
            Logger.LogInfo(model.Name);
        });

        ExcludeFolderCommand = ReactiveCommand.Create<ItemGroupState>((groupModels) =>
        {
            Logger.LogInfo(groupModels.Name);
        });

        AddProjectItemCommand = ReactiveCommand.CreateFromTask<ItemGroupState>(async (groupModels) =>
        {
            Logger.LogInfo(groupModels.Name);

            var addItemViewModel = new AddItemViewModel();

            var dialogResult = await NavigateDialogAsync(addItemViewModel,
                NavigationTarget.DialogScreen);

            ItemGroups.FirstOrDefault(groupModels)
            .Items.Add(dialogResult.Result);
        });

        Task.Run(async () =>
        {
            var result = new ObservableCollectionExtended<UFModel>(
           (await queryDispatcher
         .Dispatch<GetAllUFQuery, IList<UFModel>>(new(), CancellationToken.None)
         ).OrderBy(x => x.Name));

            Dispatcher.UIThread.Post(() => UfList = result);
        });
    }
    public ICommand ExcludeFileCommand
    {
        get; set;
    }
    public ICommand RenameFileCommand
    {
        get; set;
    }
    public ICommand ExcludeFolderCommand
    {
        get; set;
    }
    public ICommand AddProjectItemCommand
    {
        get; set;
    }
    public ICommand ChooseSolutionPath
    {
        get;
    }
    public ICommand ChooseLogoPath
    {
        get;
    }
}

public static class Extension
{
    public static ProjectSolutionModel ToSolutionModel(this SolutionStateViewModel model)
    {
        return new()
        {
            FileName = model.FileName,
            FilePath = model.FilePath,
            ItemGroups = model
            .ItemGroups
            .Select(item => new ItemGroupModel()
            {
                Name = item.Name,
                Items = item
                .Items
                .Select(child => new ItemModel()
                {
                    Id = child.Id,
                    Name = child.Name,
                    ItemPath = child.ItemPath,
                    TemplateName = child.TemplateName
                }).ToList()
            })
            .ToList(),
            SolutionReportInfo = model.ReportData,
            ParentFolderName = model.ParentFolderName,
            ParentFolderPath = model.ParentFolderPath
        };
    }
    public static SolutionStateViewModel ToSolutionState(this ProjectSolutionModel model)
    {
        return new()
        {
            FileName = model.FileName,
            FilePath = model.FilePath,
            ItemGroups = new(model
            .ItemGroups
            .Select(item => new ItemGroupState()
            {
                Name = item.Name,
                Items = new(item
                .Items
                .Select(child => new ItemState()
                {
                    Id = child.Id,
                    Name = child.Name,
                    ItemPath = child.ItemPath,
                    TemplateName = child.TemplateName
                }))
            })),
            ReportData = model.SolutionReportInfo,
            ParentFolderName = model.ParentFolderName,
            ParentFolderPath = model.ParentFolderPath
        };
    }
}
