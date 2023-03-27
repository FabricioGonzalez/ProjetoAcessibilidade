using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

using DynamicData;
using DynamicData.Binding;

using Project.Domain.App.Queries.GetAllTemplates;
using Project.Domain.Contracts;
using Project.Domain.Project.Commands.ProjectItemCommands.SaveCommands;

using ProjectAvalonia.Common.Extensions;
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
    [AutoNotify] private int _selectedTab;
    [AutoNotify] private ReadOnlyObservableCollection<FileItem> _items;
    public ObservableCollectionExtended<FileItem> Source
    {
        get;
    } = new();

    private readonly IQueryDispatcher queryDispatcher;
    private readonly ICommandDispatcher commandDispatcher;

    public async Task LoadItems()
    {
        var result = await queryDispatcher
            .Dispatch<GetAllTemplatesQuery, Resource<List<ExplorerItem>>>(
            query: new(),
            cancellation: CancellationToken.None);

        result.OnError((res) =>
        {

        })
        .OnLoadingStarted((res) =>
        {
        })
        .OnSuccess((res) =>
        {
            if (res.Data is not null)
            {
                Source.Load(
                    res.Data.Select(item => new FileItem()
                    {
                        Name = item.Name,
                        FilePath = item.Path
                    }));
            }
        });
    }

    public TemplateEditViewModel()
    {
        queryDispatcher = Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher = Locator.Current.GetService<ICommandDispatcher>();

        Source.ToObservableChangeSet()
             .ObserveOn(RxApp.MainThreadScheduler)
             .Bind(out _items)
             .Subscribe();

        SelectionMode = NavBarItemSelectionMode.Button;

        _selectedTab = 0;

        TemplateEditTab = new TemplateEditTabViewModel();


        this.WhenPropertyChanged(vm => vm.TemplateEditTab.SelectedItem)
            .SubscribeAsync(async prop =>
            {
                var dialog = new DeleteDialogViewModel(
               message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item", caption: "");

                if (prop.Value is not null)
                {
                    if ((await NavigateDialogAsync(dialog, target: NavigationTarget.CompactDialogScreen)).Result == true)
                    {
                        Logger.LogDebug(prop.Value.FilePath);
                    }
                }
            });

        AddNewItemCommand = ReactiveCommand.Create(() =>
        {
            Source.Add(new FileItem() { InEditMode = true });
        });

        ExcludeItemCommand = ReactiveCommand.CreateFromTask<FileItem>(async (item) =>
        {
            var dialog = new DeleteDialogViewModel(
                message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item", caption: "");

            if ((await NavigateDialogAsync(dialog, target: NavigationTarget.CompactDialogScreen)).Result == true)
            {
                /*Logger.LogInfo(groupModels.Name);*/
            }
        });

        RenameItemCommand = ReactiveCommand.Create<FileItem>((item) =>
        {
            item.InEditMode = true;
        });

        CommitItemCommand = ReactiveCommand.Create<FileItem>((item) =>
        {
            var result = Items.Single(file => file.Name == item.Name);

            if (result is not null)
            {
                result.FilePath = System.IO.Path
                .Combine(Constants.AppItemsTemplateFolder, $"{result.Name}{Constants.AppProjectTemplateExtension}");

                var appItem = new AppItemModel()
                {
                    ItemName = result.Name,
                    TemplateName = result.Name,
                    Id = Guid.NewGuid().ToString(),
                    LawList = new List<AppLawModel>(),
                    FormData = new List<IAppFormDataItemContract>()
                    { new AppFormDataItemObservationModel()
                    { Type = Core.Enuns.AppFormDataType.Observação,
                Observation = "",
                Topic = "Observações"},
                    new AppFormDataItemImageModel(){
                    Topic = "Imagens",
                    Type = Core.Enuns.AppFormDataType.Image,
                    ImagesItems = new List<ImagesItem>()
                    } }
                };

                if (!IoHelpers.CheckIfFileExists(result.FilePath))
                    commandDispatcher.Dispatch<SaveSystemProjectItemContentCommand, Resource<MediatR.Unit>>(
                        new(appItem, result.FilePath),
                        CancellationToken.None);

                Logger.LogDebug(result.Name);
            }
        });

        (CommitItemCommand as ReactiveCommand<FileItem, System.Reactive.Unit>).ThrownExceptions.Subscribe(exception =>
    {
        Source.Remove(Source.Last());
    });

    }
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
}
