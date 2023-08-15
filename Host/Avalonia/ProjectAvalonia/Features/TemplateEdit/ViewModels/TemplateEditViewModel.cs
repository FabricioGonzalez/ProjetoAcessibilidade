using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Common.Optional;
using DynamicData.Binding;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.Project.Services;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Features.TemplateEdit.ViewModels.Components;
using ProjectAvalonia.Models;
using ProjectAvalonia.Presentation.Interfaces;
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
    private readonly ItemsService _itemsService;
    private readonly ValidationRulesService _validationRulesService;

    private IEditableItemViewModel _selectedItem;

    public TemplateEditViewModel(
        ITemplateEditTabViewModel templateEditTab
        , ItemValidationViewModel itemValidationTab
        , ItemsService itemsService
        , EditableItemService _editableItemService
        , ValidationRulesService validationRulesService
    )
    {
        _itemsService = itemsService;
        this._editableItemService = _editableItemService;
        _validationRulesService = validationRulesService;
        SetupCancel(
            enableCancel: false,
            enableCancelOnEscape: true,
            enableCancelOnPressed: true);

        SelectionMode = NavBarItemSelectionMode.Button;

        var changeSet = Items?.ToObservableChangeSet();

        CommitItemCommand = ReactiveCommand.CreateFromTask<IEditableItemViewModel>(CreateItem);

        ToolBar = new MenuViewModel(CreateMenu().ToImmutable());

        TemplateEditTab = templateEditTab;
        ItemValidationTab = itemValidationTab;

        _ = this.WhenAnyValue(x => x.SelectedItem)
            .WhereNotNull()
            .InvokeCommand(LoadSelectedItem);

        AddNewItemCommand = ReactiveCommand.Create(
            () =>
            {
            });
        LoadAllItems = ReactiveCommand.CreateRunInBackground(
            execute: LoadItems,
            outputScheduler: RxApp.MainThreadScheduler);

        ExcludeItemCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                var dialog = new DeleteDialogViewModel(
                    message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item"
                    , caption: "");

                if ((await NavigateDialogAsync(dialog: dialog,
                        target: NavigationTarget.CompactDialogScreen)).Result)
                {
                    if (SelectedItem is { } item)
                    {
                        Items?.Remove(item);

                        /*await _mediator.Send(request: new DeleteProjectFileItemCommand(item.ItemPath)
                            , cancellation: CancellationToken.None);*/
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
    }


    public override MenuViewModel? ToolBar
    {
        get;
    }

    private ReactiveCommand<IEditableItemViewModel, Unit> LoadSelectedItem =>
        ReactiveCommand.CreateRunInBackground<IEditableItemViewModel>(async item =>
        {
            await LoadItemReport(path: item.ItemPath, itemTemplateName: item.TemplateName);
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
        set => this.RaiseAndSetIfChanged(backingField: ref _selectedItem, newValue: value);
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
            label: "Template_Edit_Create_Item_ToolBarItem".GetLocalized(),
            command: ReactiveCommand.Create(() =>
            {
                Items?.Add(new EditableItemViewModel(CommitItemCommand)
                {
                    Id = Guid.NewGuid().ToString(), InEditMode = true, Name = "", TemplateName = "", ItemPath = ""
                });
            }),
            icon: "solution_create_24_rounded".GetIcon(),
            gesture: "Ctrl+Shift+N"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
            label: "Template_Edit_Save_Item_ToolBarItem".GetLocalized(),
            command: ReactiveCommand.CreateRunInBackground(async () =>
            {
                await TemplateEditTab.EditingItem.ToOption()
                    .Map(async item =>
                    {
                        await SaveItemData(item);
                    })
                    .Reduce(() => Task.CompletedTask);
            }),
            icon: "save_data_24_rounded".GetIcon(),
            gesture: "Ctrl+S"));

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

    /*var itemTemplate = _mediator.Send(
            request: new GetSystemProjectItemContentQuery(path),
            cancellation: CancellationToken.None);

        var templateRules =
            LoadValidationRules(Path.Combine(path1: Constants.AppValidationRulesTemplateFolder,
                path2: $"{Path.GetFileNameWithoutExtension(path)}{Constants.AppProjectValidationTemplateExtension}"));

        await Task.WhenAll(itemTemplate, templateRules);
        var res = templateRules.Result;
    itemTemplate
            .Result
            .Match(success =>
                {
                    var item = success
                        ?.ToAppStateFillable();

                    item?.FormData.IterateOn(formItem =>
                    {
                        if (formItem.Body is CheckboxContainerItemState CheckboxItemContainer)
                        {
                            CheckboxItemContainer.Children.IterateOn(child =>
                            {
                                child.ValidationRules = templateRules.Result;
                            });
                        }

                        if (formItem.Body is TextItemState textItem)
                        {
                            textItem.ValidationRules = templateRules.Result;
                        }
                    });

                    TemplateEditTab.EditingItem = item;
                    TemplateEditTab.EditingItemRules = res;

                    ItemValidationTab.ValidationItemRules = res;

                    return item;
                },
                it =>
                {
                    NotificationHelpers.Show(it);
                    return default;
                });
    private async Task<ObservableCollection<IValidationRuleContainerState>> LoadValidationRules(
        string path
    ) => new();
    (await _mediator.Send(
        request: new GetValidationRulesQuery(path),
        cancellation: CancellationToken.None))
    .Match(Succ: success =>
        {
            var result = success.ToList().Select(x =>
            {
                return new ValidationRuleContainerState
                {
                    TargetContainerId = x.Target.Id, TargetContainerName = x.Target.Name, ValidaitonRules =
                        new ObservableCollection<IValidationRuleState>(x.Rules.Select(y =>
                            new ValidationRuleState
                            {
                                ValidationRuleName = y.RuleName
                                , Type = AppValidation.GetOperationByValue(y.Operation)
                                , Conditions = new ObservableCollection<IConditionState>(y.Conditions.Select(cond =>
                                    new ConditionState
                                    {
                                        TargetId = cond.TargetId
                                        , Result = new ObservableCollection<Result>(
                                            cond.Result.Select(it => new Result { ResultValue = it }))
                                        , CheckingValue =
                                            AppValidation.GetCheckingOperationByValue(cond.Type) is IsOperation
                                                ? AppValidation.GetCheckingValueByValue(cond.CheckingValue)
                                                : new TextType(cond.CheckingValue)
                                        , CheckingOperationType =
                                            AppValidation.GetCheckingOperationByValue(cond.Type)
                                    }))
                            }))
                };
            });

            return new ObservableCollection<IValidationRuleContainerState>(result);
        }
        , Fail: failure =>
        {
            NotificationHelpers.Show(title: "Erro ao ler regras", message: failure.Message);

            return new ObservableCollection<IValidationRuleContainerState>(Enumerable
                .Empty<IValidationRuleContainerState>());
        }
    )*/

    private void LoadItems()
    {
        Items = new ObservableCollection<IEditableItemViewModel>(_itemsService.LoadAllItems().Select(it =>
            new EditableItemViewModel(CommitItemCommand)
            {
                Id = it.Id, TemplateName = it.TemplateName, ItemPath = it.ItemPath, Name = it.Name
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
        itemContent.ItemTemplate = item.TemplateName;

        itemContent.FormData = new ObservableCollection<FormItemContainer>();

        itemContent.LawItems = new ObservableCollection<LawStateItem>();

        await _editableItemService.CreateTemplateEditingItem(itemContent);
    }

    private async Task SaveItemData(
        AppModelState item
    )
    {
        item.ItemName = item.ItemTemplate;
        item.Id = Guid.NewGuid().ToString();

        await _editableItemService.CreateTemplateEditingItem(item);
        await _validationRulesService.CraeteRules(rules: TemplateEditTab.EditingItemRules, itemName: item.ItemTemplate);
    }
}