using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Common.Linq;

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
    private readonly EditingItemsNavigationService _editableItemsNavigationService;

    public ProjectEditingViewModel(
        SolutionService solutionService
        , EditableItemService editableItemService
        , ValidationRulesService validationRulesService,
        EditingItemsNavigationService editableItemsNavigationService)
    {
        _solutionService = solutionService;
        _editableItemService = editableItemService;
        _validationRulesService = validationRulesService;
        _editableItemsNavigationService = editableItemsNavigationService;


        _editableItemsNavigationService.ExportList()
            .Bind(out _editingItems)
            .Subscribe();

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
                    _editableItemsNavigationService.RemoveItem(x);
                    SelectedItem = null;
                    this.RaisePropertyChanged(nameof(SelectedItem));
                    return;
                }

                /*var dialog = new DeleteDialogViewModel(
                    message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item"
                    , caption: "");

                if ((await RoutableViewModel.NavigateDialogAsync(dialog: dialog,
                        target: NavigationTarget.CompactDialogScreen)).Result.Item2)
                {
                    _editableItemsNavigationService.RemoveItem(x);
                    SelectedItem = null;
                    this.RaisePropertyChanged(nameof(SelectedItem));
                }*/
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

    public ReadOnlyObservableCollection<IEditingItemViewModel> _editingItems;
    public ReadOnlyObservableCollection<IEditingItemViewModel> EditingItems => _editingItems;

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


        if (editing is { } item)
        {
            if (EditingItems.FirstOrDefault(x => x.ItemPath == editing.ItemPath) is { } editingItem)
            {
                SelectedItem = editingItem;
                this.RaisePropertyChanged(nameof(SelectedItem));
                return;
            }

            if (item is SolutionItemViewModel solution)
            {
                var result = _solutionService.GetSolution(solution.ItemPath);

                var solutionItem = new SolutionEditItemViewModel(itemName: solution.Name, id: solution.Id
                    , itemPath: solution.ItemPath
                    , body: new SolutionItemBody(result), isSaved: false);

                _editableItemsNavigationService.AddItem(solutionItem);
            }

            if (item is ConclusionItemViewModel conclusion)
            {
                var conclusionItem = new ConclusionEditItemViewModel(itemName: conclusion.Name, id: conclusion.Id
                    , itemPath: conclusion.ItemPath
                    , body: new ConclusionEditingBody(), isSaved: false);

                _editableItemsNavigationService.AddItem(conclusionItem);

            }

            if (item is ItemViewModel edit)
            {
                var getItem = await _editableItemService.GetEditingItem(edit.ItemPath);
                var rules = await _validationRulesService.LoadRulesByName(edit.TemplateName);

                if (!string.IsNullOrWhiteSpace(getItem.ItemName) && getItem.FormData is not null)
                {
                    var observations = new SourceList<ObservationState>();
                    observations.AddRange(getItem.ObservationContainer.Observations.Select(it => new ObservationState() { Observation = it.Observation }));

                    _editableItemsNavigationService.AddItem(getItem.ToEditingView(edit.ItemPath, rules.Cast<ValidationRuleContainerState>()
                        , observations));
                }
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
                        .Where(image => !string.IsNullOrWhiteSpace(image.ImagePath))
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
                    ,
                    CheckboxContainerItemState checkbox => new CheckboxFormItem(id: checkbox.Id,
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
                                                    value: option.Value
                                                    , isInvalid: option.IsInvalid,
                                                    isChecked: option.IsChecked))))))))
                    ,
                    _ => throw new ArgumentOutOfRangeException(paramName: nameof(item), actualValue: item
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

        appModel.FormData = new();
        appModel.Images = new();
        appModel.Observations = new();
        foreach (var formItem in viewModel.Form)
        {
            if (formItem is IImageFormItemViewModel image)
            {
                image.ImageItems.IterateOn(it =>
                {
                    if (!string.IsNullOrWhiteSpace(it.ImagePath) && File.Exists(it.ImagePath))
                    {
                        appModel.Images.Add(new ImageItem { Id = it.Id, ImagePath = it.ImagePath, ImageObservation = it.ImageObservation });
                    }
                });
            }
            if (formItem is IObservationFormItemViewModel observation)
            {

                observation.Observations.IterateOn(it =>
                {
                    if (!string.IsNullOrWhiteSpace(it.Observation))
                        appModel.Observations.Add(new ObservationModel { Id = Guid.NewGuid().ToString(), Observation = it.Observation });
                });
            }

            if (formItem is TextFormItemViewModel text)
            {
                appModel.FormData?.Add(new ItemFormDataTextModel(id: text.Id, topic: text.Topic,
                    textData: text.TextData,
                    measurementUnit: text.MeasurementUnit ?? ""));
            }

            if (formItem is CheckboxFormItem checkbox)
            {
                appModel.FormData?.Add(new ItemFormDataCheckboxModel(id: checkbox.Id, topic: checkbox.Topic)
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
                            ,
                            Options = child.Options.Options.Select(option =>
                                new ItemOptionModel(
                                    id: option.Id,
                                    value: option.Value,
                                    isChecked: option.IsChecked,
                                    isInvalid: option.IsInvalid)).ToList()
                        }).ToList()
                });
            }
        }
      
        appModel.LawList = viewModel.LawList.Select(x => new ItemLaw { LawId = x.LawId, LawTextContent = x.LawContent })
            .ToList();

        return appModel;
    }
}