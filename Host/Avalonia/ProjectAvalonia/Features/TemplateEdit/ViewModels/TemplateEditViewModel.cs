using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common;
using Common.Optional;

using DynamicData;
using DynamicData.Alias;
using DynamicData.Binding;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.TemplateEdit.ViewModels.Components;
using ProjectAvalonia.Models;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.ViewModels;

using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Images;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using ProjetoAcessibilidade.Domain.App.Queries.Templates;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Commands.SystemItems;
using ProjetoAcessibilidade.Domain.Project.Queries.SystemItems;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

[NavigationMetaData(
    Title = "Modelos",
    Caption = "Editar os modelos de relatórios do projeto",
    Order = 0,
    LocalizedTitle = "TemplateEditViewNavLabel",
    Category = "Templates",
    Searchable = true,
    Keywords = new[]
    {
        "Templates", "Edição", "Modelos"
    },
    NavBarPosition = NavBarPosition.Top,
    NavigationTarget = NavigationTarget.HomeScreen,
    IconName = "edit_regular")]
public partial class TemplateEditViewModel
    : NavBarItemViewModel,
        ITemplateEditViewModel
{
    private readonly IMediator _mediator;


    public TemplateEditViewModel(
        ITemplateEditTabViewModel templateEditTab
    )
    {
        SetupCancel(
            enableCancel: false,
            enableCancelOnEscape: true,
            enableCancelOnPressed: true);

        SelectionMode = NavBarItemSelectionMode.Button;

        var changeSet = Items?.ToObservableChangeSet();

        CommitItemCommand = ReactiveCommand.CreateFromTask<IEditableItemViewModel>(CreateItem);

        ToolBar = new MenuViewModel(CreateMenu().ToImmutable());

        TemplateEditTab = templateEditTab;
        _mediator = Locator.Current.GetService<IMediator>()!;

        this.WhenAnyValue(x => x.SelectedItem)
            .WhereNotNull()
            .InvokeCommand(LoadSelectedItem);


        AddNewItemCommand = ReactiveCommand.Create(
            () =>
            {

            });
        LoadAllItems = ReactiveCommand.CreateFromTask(
            execute: LoadItems,
            outputScheduler: RxApp.MainThreadScheduler);

        ExcludeItemCommand = ReactiveCommand.Create(
            () =>
            {
            });
        RenameItemCommand = ReactiveCommand.Create(
            () =>
            {
            });

    }


    public override MenuViewModel? ToolBar
    {
        get;
    }
    private ReactiveCommand<IEditableItemViewModel, Unit> LoadSelectedItem => ReactiveCommand.CreateFromTask<IEditableItemViewModel>(async (item) =>
    {
        var result = await _mediator.Send(
               new GetSystemProjectItemContentQuery(item.ItemPath),
               CancellationToken.None);

        result.OnSuccess(onSuccessAction: success =>
        {
            success
            ?.Data
            ?.ToAppStateFillable()
            .ToOption()
            .Map(val => TemplateEditTab.EditingItem = val);
        });
    });
    private ImmutableList<IMenuItem>.Builder CreateMenu()
    {
        var listBuilder = ImmutableList.CreateBuilder<IMenuItem>();

        listBuilder.Add(new MenuItemModel(
            label: "Open",
            command: ReactiveCommand.Create(() => { }),
            icon: "file_open_24_rounded".GetIcon(),
            "Ctrl+Shift+O"));

        listBuilder.Add(new MenuItemModel(
            label: "Create Item",
            command: ReactiveCommand.Create(() =>
            {
                Items?.Add(new EditableItemViewModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    InEditMode = true,
                    Name = "",
                    TemplateName = "",
                    ItemPath = "",
                    CommitItemCommand = CommitItemCommand
                });
            }),
            icon: "solution_create_24_rounded".GetIcon(),
            "Ctrl+Shift+N"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
           label: "Save Current Item",
          command: ReactiveCommand.CreateFromTask(async () =>
          {
              TemplateEditTab.EditingItem.ToOption()
              .Map(async (item) =>
              {
                  await SaveItemData(item);
              });
          }),
           icon: "save_data_24_rounded".GetIcon(),
           "Ctrl+S"));

        /*listBuilder.Add(new MenuItemSeparatorModel());*/

        /*listBuilder.Add(new MenuItemModel(
          label: "Print Solution",
          command: ReactiveCommand.Create(
           execute: PrintSolution,
           canExecute: isSolutionOpen),
          icon: "print_24_rounded".GetIcon(),
          "Ctrl+P"));*/

        /* listBuilder.Add(new MenuItemSeparatorModel());*/

        /* listBuilder.Add(new MenuItemModel(
          label: "Add Folder",
          command: ReactiveCommand.Create(
           execute: () =>
           {
               ProjectExplorerViewModel?.CreateFolderCommand?.Execute().Subscribe();
           },
           canExecute: isSolutionOpen),
          icon: "add_folder_24_rounded".GetIcon(),
          "Ctrl+Shift+N"));*/

        return listBuilder;
    }


    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;

    /*private readonly TemplateItemsStore _itemsStore;
    private readonly ICommandDispatcher commandDispatcher;


    [AutoNotify] private int _selectedTab;

    public TemplateEditViewModel()
    {
        commandDispatcher = Locator.Current.GetService<ICommandDispatcher>();

        _itemsStore ??= Locator.Current.GetService<TemplateItemsStore>();

        SelectionMode = NavBarItemSelectionMode.Button;

        _selectedTab = 0;

        TemplateEditTab = new TemplateEditTabViewModel();

        AddNewItemCommand = ReactiveCommand.Create(() =>
        {
            _itemsStore?.AddItem(new ItemState { InEditMode = true });
        });

        ExcludeItemCommand = ReactiveCommand.CreateFromTask<ItemState>(async item =>
        {
            var dialog = new DeleteDialogViewModel(
                "O item seguinte será excluido ao confirmar. Deseja continuar?", "Deletar Item"
                , "");

            if ((await NavigateDialogAsync(dialog, NavigationTarget.CompactDialogScreen)).Result)
            {
                /*Logger.LogInfo(groupModels.Name);#1#
            }
        });

        RenameItemCommand = ReactiveCommand.Create<ItemState>(item =>
        {
            item.InEditMode = true;
        });

        CommitItemCommand = ReactiveCommand.Create<ItemState>(item =>
        {
            var result = Items.Single(file => file.Name == item.Name);

            result.ItemPath = Path
                .Combine(Constants.AppItemsTemplateFolder
                    , $"{result.Name}{Constants.AppProjectTemplateExtension}");

            var appItem = new AppItemModel
            {
                ItemName = result.Name, TemplateName = result.Name, Id = Guid.NewGuid().ToString(),
                LawList = new List<AppLawModel>(), FormData = new List<IAppFormDataItemContract>
                {
                    new AppFormDataItemObservationModel(
                        type: AppFormDataType.Observação,
                        topic: "Observações",
                        observation: "",
                        id: ""),
                    new AppFormDataItemImageModel(
                        topic: "Imagens",
                        id: "",
                        type: AppFormDataType.Image)
                    {
                        ImagesItems = new List<ImagesItem>()
                    }
                }
            };

            if (!IoHelpers.CheckIfFileExists(result.ItemPath))
            {
                commandDispatcher.Dispatch<SaveSystemProjectItemContentCommand, Resource<Empty>>(
                    new SaveSystemProjectItemContentCommand(appItem, result.ItemPath),
                    CancellationToken.None);
            }

            Logger.LogDebug(result.Name);
        });

        (CommitItemCommand as ReactiveCommand<ItemState, Unit>)?.ThrownExceptions
            .Subscribe(exception =>
            {
                _itemsStore?.RemoveItem(Items.Last());
            });

        Dispatcher.UIThread.Post(async () =>
        {
            await _itemsStore?.LoadSystemItems(GetCancellationToken());
        });
    }

    public ReadOnlyObservableCollection<ItemState> Items => _itemsStore?.ItemsCollection;

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
    }*/
    public ObservableCollection<IEditableItemViewModel>? Items
    {
        get;
        set;
    }
    private IEditableItemViewModel _selectedItem;
    public IEditableItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public ITemplateEditTabViewModel TemplateEditTab
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> AddNewItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> LoadAllItems
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> ExcludeItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> RenameItemCommand
    {
        get;
    }

    public ReactiveCommand<IEditableItemViewModel, Unit> CommitItemCommand
    {
        get;
    }

    private async Task LoadItems() =>
        (await _mediator
            .Send(
                request: new GetAllTemplatesQuery(),
                cancellation: CancellationToken.None)).OnSuccess(
            success =>
            {
                Items = new ObservableCollection<IEditableItemViewModel>(
                    success.Data
                        ?.Select(
                            item => new EditableItemViewModel
                            {
                                Id = item.Id ??
                                     Guid.NewGuid()
                                         .ToString(),
                                ItemPath = item.ItemPath,
                                Name = "",
                                TemplateName = item.Name
                            }) ??
                    Enumerable.Empty<EditableItemViewModel>());

                this.RaisePropertyChanged(nameof(Items));
            })
        .OnError(
            _ =>
            {
            });
    private async Task CreateItem(IEditableItemViewModel item)
    {
        item.Name = item.TemplateName;
        item.ItemPath = Path.Combine(Constants.AppItemsTemplateFolder, $"{item.TemplateName}{Constants.AppProjectTemplateExtension}");

        var itemContent = new AppItemModel();
        itemContent.Id = item.Id;
        itemContent.ItemName = item.TemplateName;
        itemContent.TemplateName = item.TemplateName;

        itemContent.FormData = new List<IAppFormDataItemContract>()
        {
            new AppFormDataItemImageModel(Guid.NewGuid().ToString(),"Images"),
            new AppFormDataItemObservationModel("",Guid.NewGuid().ToString(),"Observation")
    };
        itemContent.LawList = new List<AppLawModel>();


        await _mediator
           .Send(new SaveSystemProjectItemContentCommand(itemContent, ItemPath: item.ItemPath), CancellationToken.None);
    }

    private async Task SaveItemData(
        AppModelState item
    ) =>
        await _mediator
            .Send(
                new SaveSystemProjectItemContentCommand(
                    item.ToAppModel(),
                    SelectedItem.ItemPath),
                CancellationToken.None);
}