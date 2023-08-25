using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.Features.Project.Services;
using ProjectAvalonia.Features.Project.ViewModels.Components;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Features.Project.ViewModels.EditingItemBody;
using ProjectAvalonia.Models.ValidationTypes;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.Presentation.States.LawItemState;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;
using XmlDatasource.ProjectItems.DTO;
using XmlDatasource.ProjectItems.DTO.FormItem;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ProjectEditingViewModel
    : ViewModelBase
        , IProjectEditingViewModel
{
    public static readonly Interaction<IItemViewModel, Unit> SetEditingItem = new();
    private readonly EditableItemService _editableItemService;
    private readonly SolutionService _solutionService;
    private readonly ValidationRulesService _validationRulesService;

    public ProjectEditingViewModel(
        SolutionService solutionService
        , EditableItemService editableItemService
        , ValidationRulesService validationRulesService
    )
    {
        _solutionService = solutionService;
        _editableItemService = editableItemService;
        _validationRulesService = validationRulesService;
        EditingItems = new ObservableCollection<IEditingItemViewModel>();

        AddItemToEdit = ReactiveCommand.CreateFromTask<IItemViewModel>(AddItem);

        var observableItems = EditingItems
            .ToObservableChangeSet();

        observableItems.AutoRefreshOnObservable(item => item.CloseItemCommand.IsExecuting)
            .Select(x => WhenAnyItemClosed())
            .Switch()
            .SubscribeAsync(async x =>
            {
                if (x?.IsSaved == true)
                {
                    _ = EditingItems.Remove(x);
                    return;
                }

                var dialog = new DeleteDialogViewModel(
                    message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item"
                    , caption: "");

                if ((await RoutableViewModel.NavigateDialogAsync(dialog: dialog,
                        target: NavigationTarget.CompactDialogScreen)).Result.Item2)
                {
                    EditingItems.Remove(x);
                }
            });

        observableItems
            .WhereNotNull()
            .OnItemAdded(model =>
            {
                SelectedItem = model;
                this.RaisePropertyChanged(nameof(SelectedItem));
            })
            .Subscribe();
    }

    public ReactiveCommand<IItemViewModel, Unit> AddItemToEdit
    {
        get;
    }

    public IEditingItemViewModel SelectedItem
    {
        get;
        private set;
    }

    public ObservableCollection<IEditingItemViewModel> EditingItems
    {
        get;
    }

    private IObservable<IEditingItemViewModel?> WhenAnyItemClosed() =>
        // Select the documents into a list of Observables
        // who return the Document to close when signaled,
        // then flatten them all together.
        EditingItems
            .Select(x => x.CloseItemCommand.Select(_ => x))
            .Merge();

    private async Task AddItem(
        IItemViewModel? editing
    )
    {
        if (editing is { } item && !EditingItems.Any(x => x.Id == item.Id))
        {
            if (item is SolutionItemViewModel solution)
            {
                var result = _solutionService.GetSolution(solution.ItemPath);

                var solutionItem = new SolutionEditItemViewModel(itemName: solution.Name, id: solution.Id
                    , itemPath: solution.ItemPath
                    , body: new SolutionItemBody(result), isSaved: false);

                EditingItems.Add(solutionItem);
            }

            if (item is ConclusionItemViewModel conclusion)
            {
                var conclusionItem = new ConclusionEditItemViewModel(itemName: conclusion.Name, id: conclusion.Id
                    , itemPath: conclusion.ItemPath
                    , body: new ConclusionEditingBody(), isSaved: false);

                EditingItems.Add(conclusionItem);

                /*Logger.LogDebug(item.ItemPath);*/
            }

            if (item is ItemViewModel edit)
            {
                var getItem = await _editableItemService.GetEditingItem(edit.ItemPath);
                var rules = await _validationRulesService.LoadRulesByName(edit.TemplateName);

                if (!string.IsNullOrWhiteSpace(getItem.ItemName) && getItem.FormData is not null)
                {
                    EditingItems.Add(getItem.ToEditingView(edit.ItemPath, rules.Cast<ValidationRuleContainerState>()
                        , new SourceList<ObservationState>()));
                }
            }
        }

        else
        {
            if (EditingItems.FirstOrDefault(x => !x.Id.Equals(editing.Id)) is { } editingItem)
            {
                SelectedItem = editingItem;
                this.RaisePropertyChanged(nameof(SelectedItem));
            }
        }
    }
}

public static class Extension
{
    public static IEditingItemViewModel ToEditingView(
        this AppModelState state
        , string itemPath
        , IEnumerable<IValidationRuleContainerState> rules
        , SourceList<ObservationState> observations
    ) =>
        new EditingItemViewModel(itemName: state.ItemName, id: state.Id
            , itemPath: itemPath,
            templateName: state.ItemTemplate,
            body: new EditingBodyViewModel(lawList: state.LawItems.ToViewLawList()
                , form:
                new(state.FormData.ToViewForm(rules, observations)
                    .Append(new ImageContainerFormItemViewModel(new(state.ImageContainer
                        .ImagesItems
                        .Select(it => new ImageViewModel(it.ImagePath, it.ImageObservation, it.Id))
                    ))).Append(new ObservationFormItem(observations))
                )));

    public static ObservableCollection<ILawListViewModel> ToViewLawList(
        this IEnumerable<LawStateItem> lawModels
    ) =>
        new(lawModels.Select(item =>
            new LawListViewModel(lawId: item.LawId, lawContent: item.LawContent)));

    public static ObservableCollection<IFormViewModel> ToViewForm(
        this IEnumerable<FormItemContainer> formItems
        , IEnumerable<IValidationRuleContainerState> rules
        , SourceList<ObservationState> observations
    ) =>
        new(formItems
            .Select<FormItemContainer, IFormViewModel>(item =>
            {
                return item.Body switch
                {
                    TextItemState text => new TextFormItemViewModel(
                        id: text.Id,
                        topic: text.Topic,
                        textData: text.TextData,
                        measurementUnit: text.MeasurementUnit ?? "", observations: observations,
                        rules: rules.Where(x => x.TargetContainerId == text.Id))
                    , CheckboxContainerItemState checkbox => new CheckboxFormItem(id: checkbox.Id,
                        topic: checkbox.Topic,
                        checkboxItems: new ObservableCollection<ICheckboxItemViewModel>(
                            checkbox.Children.Select(
                                child => new CheckboxItemViewModel(
                                    id: child.Id,
                                    topic: child.Topic,
                                    observations: observations,
                                    rules: rules.Where(x => x.TargetContainerId == child.Id),
                                    textItems: new ObservableCollection<ITextFormItemViewModel>(
                                        child.TextItems.Select(textItem =>
                                            new TextFormItemViewModel(id: textItem.Id,
                                                topic: textItem.Topic,
                                                textData: textItem.TextData,
                                                measurementUnit: textItem.MeasurementUnit ?? ""
                                                , observations: observations,
                                                rules: rules.Where(x => x.TargetContainerId == textItem.Id)))),
                                    options: new OptionContainerViewModel(
                                        new ObservableCollection<IOptionViewModel>(
                                            child.Options.Select(option =>
                                                new OptionItemViewModel(id: option.Id,
                                                    value: option.Value,
                                                    isChecked: option.IsChecked))))))))
                    , _ => throw new ArgumentOutOfRangeException(paramName: nameof(item), actualValue: item
                        , message: null)
                };
            }));

    public static ItemRoot ToAppModel(
        this IEditingBodyViewModel viewModel
        , string id
        , string itemName
        , string templateName
    )
    {
        var appModel = new ItemRoot();

        appModel.Id = id;
        appModel.ItemName = itemName;
        appModel.TemplateName = templateName;

        appModel.FormData = viewModel.Form
            .Where(x => x is not IImageFormItemViewModel && x is not IObservationFormItemViewModel)
            .Select<IFormViewModel, ItemFormDataContainer>(formData =>
            {
                return formData switch
                {
                    TextFormItemViewModel text => new ItemFormDataTextModel(id: text.Id, topic: text.Topic,
                        textData: text.TextData,
                        measurementUnit: text.MeasurementUnit ?? "")
                    , CheckboxFormItem checkbox => new ItemFormDataCheckboxModel(id: checkbox.Id, topic: checkbox.Topic)
                    {
                        Children = checkbox.CheckboxItems.Select(
                            child => new ItemFormDataCheckboxChildModel(id: child.Id, topic: child.Topic
                                , isInvalid: child.IsInvalid)
                            {
                                TextItems = child.TextItems.Select(textItem =>
                                    new ItemFormDataTextModel(
                                        id: textItem.Id,
                                        topic: textItem.Topic,
                                        textData: textItem.TextData,
                                        measurementUnit: textItem.MeasurementUnit ?? "")).ToList()
                                , Options = child.Options.Options.Select(option =>
                                    new ItemOptionModel(
                                        id: option.Id,
                                        value: option.Value,
                                        isChecked: option.IsChecked)).ToList()
                            }).ToList()
                    }
                    , _ => throw new ArgumentOutOfRangeException(paramName: nameof(formData), actualValue: formData
                        , message: null)
                };
            }).ToList();

        appModel.Images = viewModel
            .Form
            .Where(x => x is IImageFormItemViewModel)
            .Cast<IImageFormItemViewModel>()
            .SelectMany(x => x.ImageItems)
            .Select(x => new ImageItem { Id = x.Id, ImagePath = x.ImagePath, ImageObservation = x.ImageObservation })
            .ToList();

        var result = viewModel
            .Form
            .Where(x => x is IObservationFormItemViewModel)
            .Cast<IObservationFormItemViewModel>();

        appModel.Observations = result
            .SelectMany(x => x.Observations)
            .Where(it => it.Observation.Length > 0)
            .Select(it => new ObservationModel { Id = Guid.NewGuid().ToString(), Observation = it.Observation })
            .ToList();

        appModel.LawList = viewModel.LawList.Select(x => new ItemLaw { LawId = x.LawId, LawTextContent = x.LawContent })
            .ToList();

        return appModel;
    }
}