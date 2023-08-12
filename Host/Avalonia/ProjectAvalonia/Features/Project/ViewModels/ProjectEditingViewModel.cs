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
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.Features.Project.Services;
using ProjectAvalonia.Features.Project.ViewModels.Components;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Features.Project.ViewModels.EditingItemBody;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.Presentation.States.LawItemState;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ProjectEditingViewModel
    : ViewModelBase
        , IProjectEditingViewModel
{
    public static readonly Interaction<IItemViewModel, Unit> SetEditingItem = new();
    private readonly EditableItemService _editableItemService;
    private readonly SolutionService _solutionService;

    public ProjectEditingViewModel(
        SolutionService solutionService
        , EditableItemService _editableItemService
    )
    {
        _solutionService = solutionService;
        this._editableItemService = _editableItemService;
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
                        target: NavigationTarget.CompactDialogScreen)).Result)
                {
                    _ = EditingItems.Remove(x);
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
        IItemViewModel item
    )
    {
        if (!EditingItems.Any(x => x.Id == item.Id))
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

                Logger.LogDebug(item.ItemPath);
            }

            if (item is ItemViewModel edit)
            {
                var getItem = await _editableItemService.GetEditingItem(edit.ItemPath);

                EditingItems.Add(getItem.ToEditingView(edit.ItemPath));
                /*var getItem = _mediator.Send(
                    request: new GetProjectItemContentQuery(edit.ItemPath),
                    cancellation: CancellationToken.None);

                var getRules = _mediator.Send(
                    request: new GetValidationRulesQuery(Path.Combine(path1: Constants.AppValidationRulesTemplateFolder,
                        path2: $"{edit.TemplateName}{Constants.AppProjectValidationTemplateExtension}")),
                    cancellation: CancellationToken.None);

                await Task.WhenAll(getItem, getRules);

                getItem.Result.IfSucc(successData =>
                {
                    getRules.Result.IfSucc(rules =>
                    {
                        var observations = new ObservationFormItem();

                        observations.SourceItems.AddRange(
                            successData.Observations.Where(it => it.ObservationText.Length > 0));

                        /*IEditingItemViewModel itemToEdit = new EditingItemViewModel(
                            id: edit.Id,
                            itemName: successData.ItemName,
                            itemPath: edit.ItemPath,
                            body: new EditingBodyViewModel(
                                lawList: successData.LawList.ToViewLawList(),
                                form: new ObservableCollection<IFormViewModel>(successData.FormData
                                    .ToViewForm(rules, observations.SourceItems)
                                    .Append(observations)
                                    .Append(new ImageContainerFormItemViewModel(
                                        imageItems: new ObservableCollection<IImageItemViewModel>(
                                            successData.Images
                                                .Select(x => new ImageViewModel(id: x.Id, imagePath: x.ImagePath,
                                                    imageObservation: x.ImageObservation))), topic: "Imagens")))));
                        EditingItems.Add(itemToEdit);#1#
                    });
                });*/
            }
        }

        else
        {
            if (EditingItems.FirstOrDefault(x => !x.Id.Equals(item.Id)) is { } editingItem)
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
    ) =>
        new EditingItemViewModel(itemName: state.ItemName, id: state.Id
            , itemPath: itemPath,
            body: new EditingBodyViewModel(lawList: state.LawItems.ToViewLawList()
                , form: state.FormData.ToViewForm()));

    public static ObservableCollection<ILawListViewModel> ToViewLawList(
        this IEnumerable<LawStateItem> lawModels
    ) =>
        new(lawModels.Select(item =>
            new LawListViewModel(lawId: item.LawId, lawContent: item.LawContent)));

    public static ObservableCollection<IFormViewModel> ToViewForm(
        this IEnumerable<FormItemContainer> formItems
        /*, IEnumerable<ValidationRule> rules
        , SourceList<ObservationModel> observations*/
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
                        measurementUnit: text.MeasurementUnit ?? "" /*, observations: observations,
                        rules: rules.Where(x => x.Target.Id == text.Id)*/)
                    , CheckboxContainerItemState checkbox => new CheckboxFormItem(id: checkbox.Id,
                        topic: checkbox.Topic,
                        checkboxItems: new ObservableCollection<ICheckboxItemViewModel>(
                            checkbox.Children.Select(
                                child => new CheckboxItemViewModel(
                                    id: child.Id,
                                    topic: child.Topic,
                                    /*observations: observations,
                                    rules: rules.Where(x => x.Target.Id == child.Id),*/
                                    textItems: new ObservableCollection<ITextFormItemViewModel>(
                                        child.TextItems.Select(textItem =>
                                            new TextFormItemViewModel(id: textItem.Id,
                                                topic: textItem.Topic,
                                                textData: textItem.TextData,
                                                measurementUnit: textItem.MeasurementUnit ?? ""
                                                /*,observations: observations,
                                                rules: rules.Where(x => x.Target.Id == child.Id))*/))),
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

    /*public static AppItemModel ToAppModel(
        this IEditingBodyViewModel viewModel
    )
    {
        var appModel = new AppItemModel();

        appModel.FormData = viewModel.Form
            .Where(x => x is not IImageFormItemViewModel && x is not IObservationFormItemViewModel)
            .Select<IFormViewModel, IAppFormDataItemContract>(formData =>
            {
                return formData switch
                {
                    TextFormItemViewModel text => new AppFormDataItemTextModel(text.Id, text.Topic,
                        textData: text.TextData,
                        measurementUnit: text.MeasurementUnit ?? "")
                    , CheckboxFormItem checkbox => new AppFormDataItemCheckboxModel(checkbox.Id, checkbox.Topic)
                    {
                        Children = checkbox.CheckboxItems.Select(
                            child => new AppFormDataItemCheckboxChildModel(child.Id, child.Topic, child.IsInvalid)
                            {
                                TextItems = child.TextItems.Select(textItem =>
                                    new AppFormDataItemTextModel(
                                        textItem.Id,
                                        textItem.Topic,
                                        textData: textItem.TextData,
                                        measurementUnit: textItem.MeasurementUnit ?? ""))
                                , Options = child.Options.Options.Select(option =>
                                    new AppOptionModel(
                                        id: option.Id,
                                        value: option.Value,
                                        isChecked: option.IsChecked))
                            })
                    }
                    , _ => throw new ArgumentOutOfRangeException(nameof(formData), formData, null)
                };
            });

        appModel.Images = viewModel
            .Form
            .Where(x => x is IImageFormItemViewModel)
            .Cast<IImageFormItemViewModel>()
            .SelectMany(x => x.ImageItems)
            .Select(x => new ImagesItem { Id = x.Id, ImagePath = x.ImagePath, ImageObservation = x.ImageObservation });

        var result = viewModel
            .Form
            .Where(x => x is IObservationFormItemViewModel)
            .Cast<IObservationFormItemViewModel>();

        appModel.Observations = result.SelectMany(x => x.Observations).Where(it => it.ObservationText.Length > 0);

        appModel.LawList = viewModel.LawList.Select(x => new AppLawModel(x.LawId, x.LawContent));

        return appModel;
    }*/
}