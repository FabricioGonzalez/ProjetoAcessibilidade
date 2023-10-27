using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Common;
using Common.Optional;
using DynamicData.Binding;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.Project.Services;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Features.TemplateEdit.Services;
using ProjectAvalonia.Features.TemplateEdit.ViewModels.Components;
using ProjectAvalonia.Models;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.Interfaces.Services;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.Presentation.States.LawItemState;
using ProjectAvalonia.ViewModels;
using ReactiveUI;

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
    : NavBarItemViewModel
        , ITemplateEditViewModel
{
    private readonly EditableItemService _editableItemService;
    private readonly ExportTemplateService _exportTemplateService;
    private readonly ImportDialogViewModel _importDialogViewModel;
    private readonly ItemsService _itemsService;
    private readonly ValidationRulesService _validationRulesService;

    private IEditableItemViewModel? _selectedItem;

    public TemplateEditViewModel(
        ITemplateEditTabViewModel templateEditTab
        , ItemValidationViewModel itemValidationTab
        , ItemsService itemsService
        , EditableItemService editableItemService
        , ValidationRulesService validationRulesService
        ,
        ImportTemplateService importTemplateService,
        ExportTemplateService exportTemplateService,
        IFilePickerService filePickerService)
    {
        _itemsService = itemsService;
        _editableItemService = editableItemService;
        _validationRulesService = validationRulesService;
        _exportTemplateService = exportTemplateService;
        var importTemplateService1 = importTemplateService;

        _importDialogViewModel = new ImportDialogViewModel(importTemplateService1, filePickerService);

        SetupCancel(
            false,
            true,
            true);

        SelectionMode = NavBarItemSelectionMode.Button;

        var changeSet = Items?.ToObservableChangeSet();

        CommitItemCommand = ReactiveCommand.CreateFromTask<IEditableItemViewModel>(CreateItem);

        ToolBar = new MenuViewModel(CreateMenu().ToImmutable());

        TemplateEditTab = templateEditTab;
        ItemValidationTab = itemValidationTab;

        this.WhenAnyValue(x => x.SelectedItem)
            .WhereNotNull()
            .InvokeCommand(LoadSelectedItem);

        AddNewItemCommand = ReactiveCommand.Create(
            () =>
            {
            });
        LoadAllItems = ReactiveCommand.CreateRunInBackground(
            LoadItems,
            outputScheduler: RxApp.MainThreadScheduler);

        ExcludeItemCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                var dialog = new DeleteDialogViewModel(
                    "O item seguinte será excluido ao confirmar. Deseja continuar?", "Deletar Item"
                    , "");

                if ((await NavigateDialogAsync(dialog,
                        NavigationTarget.CompactDialogScreen)).Result.Item2)
                {
                    if (SelectedItem is { } item)
                    {
                        Items?.Remove(item);

                        var path = Path.Combine(Constants.AppItemsTemplateFolder
                            , $"{item?.TemplateName ?? item.Name}{Constants.AppProjectTemplateExtension}");

                        _itemsService.ExcludeFile(path);
                    }
                }
            });
        RenameItemCommand = ReactiveCommand.Create(
            () =>
            {
                if (SelectedItem is { InEditMode: false } item)
                {
                    item.InEditMode = true;
                }
            });
        _exportTemplateService = exportTemplateService;
    }


    public override MenuViewModel? ToolBar
    {
        get;
    }

    private ReactiveCommand<IEditableItemViewModel, Unit> LoadSelectedItem =>
        ReactiveCommand.CreateRunInBackground<IEditableItemViewModel>(async item =>
        {
            if (item.ItemPath is { Length: <= 0 })
            {
                item.ItemPath = Path.Combine(Constants.AppItemsTemplateFolder
                    , $"{item.TemplateName}{Constants.AppProjectTemplateExtension}");
            }

            await LoadItemReport(item.ItemPath, item.TemplateName);
        });

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;

    public ObservableCollection<IEditableItemViewModel>? Items
    {
        get;
        set;
    }

    public IEditableItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public ITemplateEditTabViewModel TemplateEditTab
    {
        get;
    }

    public IItemValidationRulesViewModel ItemValidationTab
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

    private ImmutableList<IMenuItem>.Builder CreateMenu()
    {
        var listBuilder = ImmutableList.CreateBuilder<IMenuItem>();

        /*listBuilder.Add(new MenuItemModel(
            label: "Template_Edit_Open_ToolBarItem".GetLocalized(),
            command: ReactiveCommand.Create(() =>
            {
            }),
            icon: "file_open_24_rounded".GetIcon(),
            gesture: "Ctrl+Shift+O"));*/

        listBuilder.Add(new MenuItemModel(
            "Template_Edit_Create_Item_ToolBarItem".GetLocalized(),
            ReactiveCommand.Create(() =>
            {
                Items?.Add(new EditableItemViewModel(CommitItemCommand)
                {
                    Id = Guid.NewGuid().ToString(),
                    InEditMode = true,
                    Name = "",
                    TemplateName = "",
                    ItemPath = ""
                });
            }),
            "solution_create_24_rounded".GetIcon(),
            "Ctrl+Shift+N"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
            "Template_Edit_Save_Item_ToolBarItem".GetLocalized(),
            ReactiveCommand.CreateRunInBackground(async () =>
            {
                await TemplateEditTab.EditingItem.ToOption()
                    .Map(async item =>
                    {
                        await SaveItemData(item);
                    })
                    .Reduce(() => Task.CompletedTask);
            }),
            "save_data_24_rounded".GetIcon(),
            "Ctrl+S"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
            "Template_Edit_Imports_Item_ToolBarItem".GetLocalized(),
            ReactiveCommand.CreateRunInBackground(async () =>
            {
                var result = await NavigateDialogAsync(_importDialogViewModel, NavigationTarget.DialogScreen);

                Debug.WriteLine(result.Result);
            }),
            "cloud_download_regular".GetIcon(),
            "Ctrl+I"));

        listBuilder.Add(new MenuItemModel(
            "Template_Edit_Export_Items_ToolBarItem".GetLocalized(),
            ReactiveCommand.CreateRunInBackground(async () =>
            {
                await _exportTemplateService.ExportItemsAsync();
                /*await _exportTemplateService.ExportRulesAsync();*/
            }),
            "cloud_upload_regular".GetIcon(),
            "Ctrl+E"));

        return listBuilder;
    }

    private async Task LoadItemReport(
        string path
        , string itemTemplateName
    )
    {
        TemplateEditTab.EditingItem = await _editableItemService.GetEditingItem(path);
        TemplateEditTab.EditingItemRules = new ObservableCollection<IValidationRuleContainerState>(
            await _validationRulesService.LoadRulesByName(itemTemplateName));
    }

    private void LoadItems()
    {
        Items = new ObservableCollection<IEditableItemViewModel>(_itemsService.LoadAllItems().Select(it =>
            new EditableItemViewModel(CommitItemCommand)
            {
                Id = it.Id,
                TemplateName = it.TemplateName,
                ItemPath = it.ItemPath,
                Name = it.Name
            }).ToList());

        this.RaisePropertyChanged(nameof(Items));
    }

    private async Task CreateItem(
        IEditableItemViewModel item
    )
    {
        item.Name = item.TemplateName;

        var itemContent = new AppModelState();
        itemContent.Id = item.Id;
        itemContent.ItemName = item.TemplateName;

        itemContent.FormData = new ObservableCollection<FormItemContainer>();

        itemContent.LawItems = new ObservableCollection<LawStateItem>();

        var fileName = Path.Combine(Constants.AppItemsTemplateFolder
            , $"{item.TemplateName}{Constants.AppProjectTemplateExtension}");

        var oldFile = Path.Combine(Constants.AppItemsTemplateFolder
            , $"{itemContent.ItemTemplate}{Constants.AppProjectTemplateExtension}");
        IoHelpers.EnsureContainingDirectoryExists(oldFile);

        if (IoHelpers.CheckIfFileExists(oldFile))
        {
            _itemsService.RenameFile(
                oldFile,
                fileName
            );
        }
        else
        {
            File.Create(fileName);
        }

        itemContent.ItemTemplate = item.TemplateName;

        await _editableItemService.CreateTemplateEditingItem(itemContent);
        await _validationRulesService.CraeteRules(TemplateEditTab.EditingItemRules, item.TemplateName);
    }

    private async Task SaveItemData(
        AppModelState item
    )
    {
        item.ItemName = item.ItemTemplate;
        item.Id = Guid.NewGuid().ToString();

        await _editableItemService.CreateTemplateEditingItem(item);
        await _validationRulesService.CraeteRules(TemplateEditTab.EditingItemRules, item.ItemTemplate);
    }
}