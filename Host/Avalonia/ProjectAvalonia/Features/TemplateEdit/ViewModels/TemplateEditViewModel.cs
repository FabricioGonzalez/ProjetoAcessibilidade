using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Linq;
using Common.Optional;
using Core.Entities.Solution.Project.AppItem;
using Core.Entities.Solution.Project.AppItem.DataItems.Images;
using Core.Entities.ValidationRules;
using DynamicData.Binding;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.TemplateEdit.ViewModels.Components;
using ProjectAvalonia.Models;
using ProjectAvalonia.Models.ValidationTypes;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.ViewModels;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Images;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using ProjetoAcessibilidade.Domain.App.Queries.Templates;
using ProjetoAcessibilidade.Domain.AppValidationRules.Commands;
using ProjetoAcessibilidade.Domain.AppValidationRules.Queries;
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
    : NavBarItemViewModel
        , ITemplateEditViewModel
{
    private readonly IMediator _mediator;
    private IEditableItemViewModel _selectedItem;


    public TemplateEditViewModel(
        ITemplateEditTabViewModel templateEditTab
        , ItemValidationViewModel itemValidationTab
    )
    {
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

        _mediator = Locator.Current.GetService<IMediator>()!;

        _ = this.WhenAnyValue(x => x.SelectedItem)
            .WhereNotNull()
            .InvokeCommand(LoadSelectedItem);


        AddNewItemCommand = ReactiveCommand.Create(
            () =>
            {
            });
        LoadAllItems = ReactiveCommand.CreateFromTask(
            LoadItems,
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

    private ReactiveCommand<IEditableItemViewModel, Unit> LoadSelectedItem =>
        ReactiveCommand.CreateFromTask<IEditableItemViewModel>(async item =>
        {
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

        listBuilder.Add(new MenuItemModel(
            "Template_Edit_Open_ToolBarItem".GetLocalized(),
            ReactiveCommand.Create(() =>
            {
            }),
            "file_open_24_rounded".GetIcon(),
            "Ctrl+Shift+O"));

        listBuilder.Add(new MenuItemModel(
            "Template_Edit_Create_Item_ToolBarItem".GetLocalized(),
            ReactiveCommand.Create(() =>
            {
                Items?.Add(new EditableItemViewModel(CommitItemCommand)
                {
                    Id = Guid.NewGuid().ToString(), InEditMode = true, Name = "", TemplateName = "", ItemPath = ""
                });
            }),
            "solution_create_24_rounded".GetIcon(),
            "Ctrl+Shift+N"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
            "Template_Edit_Save_Item_ToolBarItem".GetLocalized(),
            ReactiveCommand.CreateFromTask(async () =>
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

        return listBuilder;
    }

    private async Task LoadItemReport(
        string path
        , string itemTemplateName
    )
    {
        var itemTemplate = _mediator.Send(
            new GetSystemProjectItemContentQuery(path),
            CancellationToken.None);

        var templateRules =
            LoadValidationRules(Path.Combine(Constants.AppValidationRulesTemplateFolder,
                $"{itemTemplateName}{Constants.AppProjectValidationTemplateExtension}"));

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
    }

    private async Task<ObservableCollection<IValidationRuleContainerState>> LoadValidationRules(
        string path
    ) =>
        (await _mediator.Send(
            new GetValidationRulesQuery(path),
            CancellationToken.None))
        .Match(success =>
            {
                var result = success.ToList().Select(x =>
                {
                    return new ValidationRuleContainerState
                    {
                        TargetContainerId = x.Target.Id, TargetContainerName = "", ValidaitonRules =
                            new ObservableCollection<IValidationRuleState>(x.Rules.Select(y =>
                                new ValidationRuleState
                                {
                                    Type = AppValidation.GetOperationByValue(y.Operation), ValidationRuleName = ""
                                    , Conditions = new ObservableCollection<IConditionState>(y.Conditions.Select(cond =>
                                        new ConditionState
                                        {
                                            TargetId =  cond.TargetId 
                                            , Result = new ObservableCollection<string>(cond.Result), CheckingValue =
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
            , failure =>
            {
                NotificationHelpers.Show("Erro ao ler regras", failure.Message);

                return new ObservableCollection<IValidationRuleContainerState>(Enumerable
                    .Empty<IValidationRuleContainerState>());
            }
        );

    private async Task LoadItems() =>
        (await _mediator
            .Send(
                new GetAllTemplatesQuery(),
                CancellationToken.None)).OnSuccess(
            success =>
            {
                Items = new ObservableCollection<IEditableItemViewModel>(
                    success.Data
                        ?.Select(
                            item => new EditableItemViewModel(CommitItemCommand)
                            {
                                Id = item.Id ??
                                     Guid.NewGuid()
                                         .ToString()
                                , ItemPath = item.ItemPath, Name = "", TemplateName = item.Name
                            }) ??
                    Enumerable.Empty<EditableItemViewModel>());

                this.RaisePropertyChanged(nameof(Items));
            })
        .OnError(
            _ =>
            {
            });

    private async Task CreateItem(
        IEditableItemViewModel item
    )
    {
        item.Name = item.TemplateName;
        item.ItemPath = Path.Combine(Constants.AppItemsTemplateFolder,
            $"{item.TemplateName}{Constants.AppProjectTemplateExtension}");

        var itemContent = new AppItemModel();
        itemContent.Id = item.Id;
        itemContent.ItemName = item.TemplateName;
        itemContent.TemplateName = item.TemplateName;

        itemContent.FormData = new List<IAppFormDataItemContract>
        {
            new AppFormDataItemImageModel(Guid.NewGuid().ToString(), "Images")
            , new AppFormDataItemObservationModel("", Guid.NewGuid().ToString(), "Observation")
        };
        itemContent.Images = Enumerable.Empty<ImagesItem>();
        itemContent.Observations = Enumerable.Empty<ObservationModel>();

        itemContent.LawList = new List<AppLawModel>();


        _ = await _mediator
            .Send(new SaveSystemProjectItemContentCommand(itemContent, item.ItemPath), CancellationToken.None);
    }

    private async Task SaveItemData(
        AppModelState item
    )
    {
        var itemSave = _mediator
            .Send(
                new SaveSystemProjectItemContentCommand(
                    item.ToAppModel(),
                    SelectedItem.ItemPath),
                CancellationToken.None);
        var rules = TemplateEditTab.EditingItemRules.Select(it => new ValidationRule
        {
            Target = new Target { Id = it.TargetContainerId, Name = it.TargetContainerName }, Rules =
                it.ValidaitonRules.Select(rule => new RuleSet
                {
                    Operation = rule.Type.Value
                    , Conditions = rule.Conditions.Select(condition => new Conditions(condition.TargetId
                        , condition.CheckingOperationType.Value, condition.CheckingValue.Value, condition.Result, null))
                })
        });
        var rulesSave = _mediator
                .Send(
                    new SaveValidationRulesCommand(rules
                        , Path.Combine(Constants.AppValidationRulesTemplateFolder
                            , $"{item.ItemTemplate}{Constants.AppProjectValidationTemplateExtension}")),
                    CancellationToken.None)
            ;

        await Task.WhenAll(itemSave, rulesSave);
    }
}