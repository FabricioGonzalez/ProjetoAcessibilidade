using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Common;
using Core.Entities.Solution.Explorer;
using Core.Entities.Solution.Project.AppItem;
using Core.Entities.Solution.Project.AppItem.DataItems;
using Core.Entities.Solution.Project.AppItem.DataItems.Images;
using Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using Core.Enuns;
using DynamicData;
using DynamicData.Binding;
using Project.Domain.App.Models;
using Project.Domain.App.Queries.Templates;
using Project.Domain.Contracts;
using Project.Domain.Project.Commands.SystemItems;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Logging;
using ReactiveUI;
using Splat;
using FileItem = ProjectAvalonia.Common.Models.FileItems.FileItem;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

[NavigationMetaData(
    Title = "Template Edit",
    Caption = "Edit project items templates",
    Order = 0,
    Category = "Templates",
    Searchable = true,
    Keywords = new[]
    {
        "Templates", "Editing"
    },
    NavBarPosition = NavBarPosition.Top,
    NavigationTarget = NavigationTarget.HomeScreen,
    IconName = "edit_regular")]
public partial class TemplateEditViewModel : NavBarItemViewModel
{
    private readonly ICommandDispatcher commandDispatcher;

    private readonly IQueryDispatcher queryDispatcher;
    [AutoNotify] private ReadOnlyObservableCollection<FileItem> _items;
    [AutoNotify] private int _selectedTab;

    public TemplateEditViewModel()
    {
        queryDispatcher = Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher = Locator.Current.GetService<ICommandDispatcher>();

        Source.ToObservableChangeSet()
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Bind(readOnlyObservableCollection: out _items)
            .Subscribe();

        SelectionMode = NavBarItemSelectionMode.Button;

        _selectedTab = 0;

        TemplateEditTab = new TemplateEditTabViewModel();

        AddNewItemCommand = ReactiveCommand.Create(execute: () =>
        {
            Source.Add(item: new FileItem { InEditMode = true });
        });

        ExcludeItemCommand = ReactiveCommand.CreateFromTask<FileItem>(execute: async item =>
        {
            var dialog = new DeleteDialogViewModel(
                message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item"
                , caption: "");

            if ((await NavigateDialogAsync(dialog: dialog, target: NavigationTarget.CompactDialogScreen)).Result)
            {
                /*Logger.LogInfo(groupModels.Name);*/
            }
        });

        RenameItemCommand = ReactiveCommand.Create<FileItem>(execute: item =>
        {
            item.InEditMode = true;
        });

        CommitItemCommand = ReactiveCommand.Create<FileItem>(execute: item =>
        {
            var result = Items.Single(predicate: file => file.Name == item.Name);

            if (result is not null)
            {
                result.FilePath = Path
                    .Combine(path1: Constants.AppItemsTemplateFolder
                        , path2: $"{result.Name}{Constants.AppProjectTemplateExtension}");

                var appItem = new AppItemModel
                {
                    ItemName = result.Name, TemplateName = result.Name, Id = Guid.NewGuid().ToString()
                    , LawList = new List<AppLawModel>(), FormData = new List<IAppFormDataItemContract>
                    {
                        new AppFormDataItemObservationModel(
                            type: AppFormDataType.Observação,
                            topic: "Observações",
                            observation: "",
                            id: "")
                        , new AppFormDataItemImageModel(
                            topic: "Imagens",
                            id: "",
                            type: AppFormDataType.Image)
                        {
                            ImagesItems = new List<ImagesItem>()
                        }
                    }
                };

                if (!IoHelpers.CheckIfFileExists(filePath: result.FilePath))
                {
                    commandDispatcher.Dispatch<SaveSystemProjectItemContentCommand, Resource<Empty>>(
                        command: new SaveSystemProjectItemContentCommand(AppItem: appItem, ItemPath: result.FilePath),
                        cancellation: CancellationToken.None);
                }

                Logger.LogDebug(message: result.Name);
            }
        });

        (CommitItemCommand as ReactiveCommand<FileItem, Unit>).ThrownExceptions.Subscribe(onNext: exception =>
        {
            Source.Remove(item: Source.Last());
        });
    }

    public ObservableCollectionExtended<FileItem> Source
    {
        get;
    } = new();

    public TemplateEditTabViewModel TemplateEditTab
    {
        get;
    }

    public ICommand AddNewItemCommand
    {
        get;
    }

    public ICommand ExcludeItemCommand
    {
        get;
    }

    public ICommand RenameItemCommand
    {
        get;
    }

    public ICommand CommitItemCommand
    {
        get;
    }

    public async Task LoadItems()
    {
        var result = await queryDispatcher
            .Dispatch<GetAllTemplatesQuery, Resource<List<ExplorerItem>>>(
                query: new GetAllTemplatesQuery(),
                cancellation: CancellationToken.None);

        result.OnError(onErrorAction: res =>
            {
            })
            .OnSuccess(onSuccessAction: res =>
            {
                if (res.Data is not null)
                {
                    Source.Load(
                        items: res.Data.Select(selector: item => new FileItem
                        {
                            Name = item.Name, FilePath = item.Path
                        }));
                }
            });
    }
}