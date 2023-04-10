using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using Common;
using Core.Entities.App;
using Core.Entities.Solution;
using Core.Entities.Solution.Explorer;
using Core.Entities.Solution.ItemsGroup;
using DynamicData.Binding;
using Project.Domain.App.Models;
using Project.Domain.App.Queries.UF;
using Project.Domain.Contracts;
using Project.Domain.Project.Commands.ProjectItems;
using Project.Domain.Solution.Commands.SolutionItem;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.Project.States;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Logging;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

[NavigationMetaData(Title = "Success")]
public partial class SolutionStateViewModel : RoutableViewModel
{
    private readonly ICommandDispatcher commandDispatcher;

    private readonly IQueryDispatcher queryDispatcher;

    [AutoNotify] private string _fileName = "";

    [AutoNotify] private string _filePath = "";

    [AutoNotify] private ObservableCollectionExtended<ItemGroupState> _itemGroups = new();

    [AutoNotify] private string _parentFolderName = "";

    [AutoNotify] private string _parentFolderPath = "";

    [AutoNotify] private SolutionReportState _reportData = new();

    [AutoNotify] private ObservableCollectionExtended<UFModel> _ufList;

    public SolutionStateViewModel()
    {
        queryDispatcher = Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher = Locator.Current.GetService<ICommandDispatcher>();

        ChooseSolutionPath = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var path = await FileDialogHelper.ShowOpenFolderDialogAsync(title: "Local da Solução");

            Dispatcher.UIThread.Post(action: () =>
            {
                FilePath = path;
                FileName = Path.GetFileNameWithoutExtension(path: path);
            });
        });

        ChooseLogoPath = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var path = await FileDialogHelper.ShowOpenFileDialogAsync(title: "Logo da Empresa"
                , filterExtTypes: new[] { "png" });

            Dispatcher.UIThread.Post(action: () =>
            {
                ReportData.LogoPath = path;
            });
        });

        ExcludeFileCommand = ReactiveCommand.CreateFromTask<ItemState>(execute: async model =>
        {
            var dialog = new DeleteDialogViewModel(
                message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item"
                , caption: "");

            if ((await NavigateDialogAsync(dialog: dialog, target: NavigationTarget.CompactDialogScreen)).Result)
            {
                await commandDispatcher.Dispatch<DeleteProjectFileItemCommand, Resource<Empty>>(
                    command: new DeleteProjectFileItemCommand(Item: new FileItem
                        { Name = model.Name, Path = model.ItemPath })
                    , cancellation: GetCancellationToken());
            }
        });

        RenameFileCommand = ReactiveCommand.Create<ItemState>(execute: model =>
        {
            Logger.LogInfo(message: model.Name);
        });

        ExcludeFolderCommand = ReactiveCommand.CreateFromTask<ItemGroupState>(execute: async groupModels =>
        {
            var dialog = new DeleteDialogViewModel(
                message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item"
                , caption: "");

            if ((await NavigateDialogAsync(dialog: dialog, target: NavigationTarget.CompactDialogScreen)).Result)
            {
                Logger.LogInfo(message: groupModels.Name);
            }
        });

        AddProjectItemCommand = ReactiveCommand.CreateFromTask<ItemGroupState>(execute: async groupModels =>
        {
            Logger.LogDebug(message: groupModels.Name);

            var addItemViewModel = new AddItemViewModel();

            var dialogResult = await NavigateDialogAsync(dialog: addItemViewModel,
                target: NavigationTarget.DialogScreen);

            var group = ItemGroups.FirstOrDefault(predicate: item => item.Name == groupModels.Name);

            AddItemToGroup(group: group, itemToAdd: dialogResult.Result);
        });

        CreateFolderCommand = ReactiveCommand.Create(execute: () =>
        {
            ItemGroups.Add(item: new ItemGroupState
            {
                InEditMode = true
            });
        });

        CommitFolderCommand = ReactiveCommand.CreateFromTask<ItemGroupState>(execute: async itemsGroup =>
        {
            if (itemsGroup is not null)
            {
                await commandDispatcher.Dispatch<CreateSolutionItemFolderCommand, Empty>(
                    command: new CreateSolutionItemFolderCommand(
                        ItemName: itemsGroup.Name,
                        ItemPath: itemsGroup.ItemPath)
                    , cancellation: GetCancellationToken());

                Logger.LogDebug(message: itemsGroup.Name);
            }
        });

        Task.Run(function: async () =>
        {
            var result = new ObservableCollectionExtended<UFModel>(
                collection: (await queryDispatcher
                    .Dispatch<GetAllUfQuery, IList<UFModel>>(query: new GetAllUfQuery()
                        , cancellation: GetCancellationToken())
                ).OrderBy(keySelector: x => x.Name));

            Dispatcher.UIThread.Post(action: () => UfList = result);
        });
    }

    public ICommand ExcludeFileCommand
    {
        get;
        set;
    }

    public ICommand RenameFileCommand
    {
        get;
        set;
    }

    public ICommand ExcludeFolderCommand
    {
        get;
        set;
    }

    public ICommand AddProjectItemCommand
    {
        get;
        set;
    }

    public ICommand CreateFolderCommand
    {
        get;
        set;
    }

    public ICommand CommitFolderCommand
    {
        get;
        set;
    }

    public ICommand ChooseSolutionPath
    {
        get;
    }

    public ICommand ChooseLogoPath
    {
        get;
    }

    private void AddItemToGroup(
        ItemGroupState group
        , ItemState itemToAdd
    )
    {
        if (!group.Items.Any(predicate: i => i.Name == itemToAdd.Name))
        {
            var item = itemToAdd.ItemPath.Split(separator: Path.DirectorySeparatorChar).Last().Split(separator: ".")
                .First();

            itemToAdd.ItemPath =
                Path.Combine(path1: group.ItemPath, path2: $"{item}{Constants.AppProjectItemExtension}");

            group.Items.Add(item: itemToAdd);
            CreateFileOnLocal(
                itemPath: itemToAdd.ItemPath,
                itemName: itemToAdd.TemplateName);
        }
    }

    private async Task CreateFileOnLocal(
        string itemPath
        , string itemName
    )
    {
        await commandDispatcher.Dispatch<CreateItemCommand, Resource<Empty>>(
            command: new CreateItemCommand(ItemPath: itemPath, ItemName: itemName)
            , cancellation: GetCancellationToken());

        Logger.LogDebug(message: itemPath);
    }
}

public static class Extension
{
    public static ProjectSolutionModel ToSolutionModel(
        this SolutionStateViewModel model
    ) =>
        new()
        {
            FileName = model.FileName, FilePath = model.FilePath, ItemGroups = model
                .ItemGroups
                .Select(selector: item => new ItemGroupModel
                {
                    Name = item.Name, ItemPath = item.ItemPath, Items = item
                        .Items
                        .Select(selector: child => new ItemModel
                        {
                            Id = child.Id, Name = child.Name, ItemPath = child.ItemPath
                            , TemplateName = child.TemplateName
                        }).ToList()
                })
                .ToList()
            , SolutionReportInfo = model.ReportData.ToReportData(), ParentFolderName = model.ParentFolderName
            , ParentFolderPath = model.ParentFolderPath
        };

    public static SolutionStateViewModel ToSolutionState(
        this ProjectSolutionModel model
    ) =>
        new()
        {
            FileName = model.FileName, FilePath = model.FilePath, ItemGroups =
                new ObservableCollectionExtended<ItemGroupState>(collection: model
                    .ItemGroups
                    .Select(selector: item => new ItemGroupState
                    {
                        Name = item.Name, ItemPath = item.ItemPath, Items = new ObservableCollection<ItemState>(
                            collection: item
                                .Items
                                .Select(selector: child => new ItemState
                                {
                                    Id = child.Id, Name = child.Name, ItemPath = child.ItemPath
                                    , TemplateName = child.TemplateName
                                }))
                    }))
            , ReportData = model.SolutionReportInfo.ToReportState(), ParentFolderName = model.ParentFolderName
            , ParentFolderPath = model.ParentFolderPath
        };
}