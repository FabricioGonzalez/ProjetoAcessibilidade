using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Core.Entities.Solution.Project.AppItem;
using Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using Core.Entities.Solution.Project.AppItem.DataItems.Images;
using Core.Entities.ValidationRules;
using DynamicData;
using DynamicData.Binding;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.Features.Project.ViewModels.Components;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Features.Project.ViewModels.EditingItemBody;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels.Navigation;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Text;
using ProjetoAcessibilidade.Domain.AppValidationRules.Queries;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Queries.ProjectItems;
using ProjetoAcessibilidade.Domain.Solution.Queries;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ProjectEditingViewModel
    : ViewModelBase
        , IProjectEditingViewModel
{
    public static readonly Interaction<IItemViewModel, Unit> SetEditingItem = new();
    private readonly IMediator? _mediator;

    public ProjectEditingViewModel()
    {
        _mediator = Locator.Current.GetService<IMediator>();

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
                    "O item seguinte será excluido ao confirmar. Deseja continuar?", "Deletar Item"
                    , "");

                if ((await RoutableViewModel.NavigateDialogAsync(dialog,
                        NavigationTarget.CompactDialogScreen)).Result)
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
                var result = await _mediator.Send(
                    new ReadSolutionProjectQuery(solution.ItemPath),
                    CancellationToken.None);

                result.IfSucc(success =>
                {
                    var solutionItem = new SolutionEditItemViewModel(solution.Name, solution.Id, solution.ItemPath
                        , new SolutionItemBody(success), false);

                    EditingItems.Add(solutionItem);
                });
            }

            if (item is ConclusionItemViewModel conclusion)
            {
                var conclusionItem = new ConclusionEditItemViewModel(conclusion.Name, conclusion.Id, conclusion.ItemPath
                    , new ConclusionEditingBody(), false);

                EditingItems.Add(conclusionItem);

                Logger.LogDebug(item.ItemPath);
            }

            if (item is ItemViewModel edit)
            {
                var getItem = _mediator.Send(
                    new GetProjectItemContentQuery(edit.ItemPath),
                    CancellationToken.None);

                var getRules = _mediator.Send(
                    new GetValidationRulesQuery(Path.Combine(Constants.AppValidationRulesTemplateFolder,
                        $"{edit.TemplateName}{Constants.AppProjectValidationTemplateExtension}")),
                    CancellationToken.None);

                await Task.WhenAll(getItem, getRules);

                getItem.Result.IfSucc(successData =>
                {
                    getRules.Result.IfSucc(rules =>
                    {
                        var observations = new ObservationFormItem();

                        observations.SourceItems.AddRange(
                            successData.Observations.Where(it => it.ObservationText.Length > 0));

                        IEditingItemViewModel itemToEdit = new EditingItemViewModel(
                            id: edit.Id,
                            itemName: successData.ItemName,
                            itemPath: edit.ItemPath,
                            body: new EditingBodyViewModel(
                                successData.LawList.ToViewLawList(),
                                new ObservableCollection<IFormViewModel>(successData.FormData
                                    .ToViewForm(rules, observations.SourceItems)
                                    .Append(observations)
                                    .Append(new ImageContainerFormItemViewModel(
                                        new ObservableCollection<IImageItemViewModel>(
                                            successData.Images
                                                .Select(x => new ImageViewModel(id: x.Id, imagePath: x.ImagePath,
                                                    imageObservation: x.ImageObservation))), "Imagens")))));
                        EditingItems.Add(itemToEdit);
                    });
                });
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
    public static ObservableCollection<ILawListViewModel> ToViewLawList(
        this IEnumerable<AppLawModel> lawModels
    ) =>
        new(lawModels.Select(item =>
            new LawListViewModel(item.LawId, item.LawTextContent)));

    public static ObservableCollection<IFormViewModel> ToViewForm(
        this IEnumerable<IAppFormDataItemContract> formItems
        , IEnumerable<ValidationRule> rules
        , SourceList<ObservationModel> observations
    ) =>
        new(formItems
            .Select<IAppFormDataItemContract, IFormViewModel>(item =>
            {
                return item switch
                {
                    AppFormDataItemTextModel text => new TextFormItemViewModel(
                        id: text.Id,
                        topic: text.Topic,
                        textData: text.TextData,
                        measurementUnit: text.MeasurementUnit ?? "", observations: observations,
                        rules: rules.Where(x => x.Target.Id == text.Id))
                    , AppFormDataItemCheckboxModel checkbox => new CheckboxFormItem(id: checkbox.Id,
                        topic: checkbox.Topic,
                        checkboxItems: new ObservableCollection<ICheckboxItemViewModel>(
                            checkbox.Children.Select(
                                child => new CheckboxItemViewModel(
                                    id: child.Id,
                                    topic: child.Topic,
                                    observations: observations,
                                    rules: rules.Where(x => x.Target.Id == child.Id),
                                    textItems: new ObservableCollection<ITextFormItemViewModel>(
                                        child.TextItems.Select(textItem =>
                                            new TextFormItemViewModel(id: textItem.Id,
                                                topic: textItem.Topic,
                                                textData: textItem.TextData,
                                                observations: observations,
                                                measurementUnit: textItem.MeasurementUnit ?? "",
                                                rules: rules.Where(x => x.Target.Id == child.Id)))),
                                    options: new OptionContainerViewModel(
                                        new ObservableCollection<IOptionViewModel>(
                                            child.Options.Select(option =>
                                                new OptionItemViewModel(id: option.Id,
                                                    value: option.Value,
                                                    isChecked: option.IsChecked))))))))
                    , _ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
                };
            }));

    public static AppItemModel ToAppModel(
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
    }
}