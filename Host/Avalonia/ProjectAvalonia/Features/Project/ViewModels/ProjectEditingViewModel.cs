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
using Core.Entities.ValidationRules;

using DynamicData;
using DynamicData.Binding;

using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.Features.Project.ViewModels.Components;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels.Navigation;

using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Text;
using ProjetoAcessibilidade.Domain.AppValidationRules.Queries;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Queries.ProjectItems;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ProjectEditingViewModel : ViewModelBase, IProjectEditingViewModel
{
    /*private readonly EditingItemsStore _editingItemsStore;
    private readonly ICommandDispatcher? commandDispatcher;

    private readonly IMediator? queryDispatcher;

    [AutoNotify] private int _selectedIndex;
    [AutoNotify] private ItemState _selectedItem;

    public ProjectEditingViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IMediator>();
        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();
        _editingItemsStore ??= Locator.Current.GetService<EditingItemsStore>();

        this.WhenAnyValue(property1: vm => vm.SelectedItem)
            .WhereNotNull()
            .Subscribe(onNext: prop =>
            {
                _editingItemsStore.CurrentSelectedItem = prop;
            });

        SaveItemCommand = ReactiveCommand.CreateFromTask<AppModelState?>(
            execute: async appModel =>
            {
                if (appModel is not null)
                {
                    var itemModel = appModel.ToAppModel();

                    await commandDispatcher
                        .Dispatch<SaveProjectItemContentCommand, Resource<Empty>>(
                            command: new SaveProjectItemContentCommand(
                                AppItem: itemModel, ItemPath: _editingItemsStore.CurrentSelectedItem.ItemPath),
                            cancellation: CancellationToken.None);
                }
            });

        /*AddPhotoCommand = ReactiveCommand.Create<ImageContainerItemState>(
            execute: async imageContainer =>
            {
                var file = await FileDialogHelper.ShowOpenFileDialogAsync(title: "Obter Image"
                    , filterExtTypes: new[] { "Images/*", "png", "jpeg" });

                if (!string.IsNullOrWhiteSpace(value: file))
                {
                    imageContainer.ImagesItems.Add(item: new ImageItemState { ImagePath = file });
                }
            },
            canExecute: canSave);#1#
        CloseItemCommand = ReactiveCommand.Create<ItemState>(execute: item =>
        {
            _editingItemsStore.RemoveEditingItem(item: item);
        });
    }

    public AppModelState EditingItem => _editingItemsStore?.Item;

    public ReadOnlyObservableCollection<ItemState> Items => _editingItemsStore.SelectedItems;

    public ReactiveCommand<AppModelState, Unit> SaveItemCommand
    {
        get;
    }

    public ICommand CloseItemCommand
    {
        get;
    }

    public ReactiveCommand<ImageContainerItemState, Unit> AddPhotoCommand
    {
        get;
    }*/


    public static readonly Interaction<IItemViewModel, Unit> SetEditingItem = new();
    private readonly IMediator? _mediator;

    public ProjectEditingViewModel()
    {
        _mediator = Locator.Current.GetService<IMediator>();

        EditingItems = new ObservableCollection<IEditingItemViewModel>();

        AddItemToEdit = ReactiveCommand.CreateFromObservable<IItemViewModel, Unit>(execute: AddItem);

        _ = SetEditingItem.RegisterHandler(handler: value =>
        {
            _ = AddItemToEdit.Execute(parameter: value.Input);

            value.SetOutput(output: Unit.Default);
        });

        _ = EditingItems
            .ToObservableChangeSet()
            .AutoRefreshOnObservable(reevaluator: item => item.CloseItemCommand.IsExecuting)
            .Select(selector: x => WhenAnyItemClosed())
            .Switch()
            .SubscribeAsync(onNextAsync: async x =>
            {
                if (x?.IsSaved == true)
                {
                    _ = EditingItems.Remove(item: x);
                    return;
                }

                var dialog = new DeleteDialogViewModel(
                    message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item"
                    , caption: "");

                if ((await RoutableViewModel.NavigateDialogAsync(dialog: dialog,
                        target: NavigationTarget.CompactDialogScreen)).Result)
                {
                    _ = EditingItems.Remove(item: x);
                }
            });

        _ = EditingItems
            .ToObservableChangeSet()
            .WhereNotNull()
            .OnItemAdded(addAction: model =>
            {
                SelectedItem = model;
                this.RaisePropertyChanged(propertyName: nameof(SelectedItem));
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
            .Select(selector: x => x.CloseItemCommand.Select(selector: _ => x))
            .Merge();

    private IObservable<Unit> AddItem(IItemViewModel item) =>
        Observable.Start(action: async () =>
        {
            if (EditingItems.All(predicate: x => x.Id != item.Id))
            {
                var getItem = _mediator.Send(
                     request: new GetProjectItemContentQuery(ItemPath: item.ItemPath),
                     cancellation: CancellationToken.None);

                var getRules = _mediator.Send(
                    request: new GetValidationRulesQuery(ItemPath: Path.Combine(Constants.AppValidationRulesTemplateFolder, $"{item.TemplateName}{Constants.AppProjectValidationTemplateExtension}")),
                    cancellation: CancellationToken.None);

                await Task.WhenAll(getItem, getRules);

                _ = getItem.Result.IfSucc(successData =>
                {
                    if (getRules.Result is Resource<IEnumerable<ValidationRule>>.Success rules)
                    {
                        var observations = new ObservationFormItem();

                        observations.SourceItems.AddRange(successData.Observations.Where(it => it.ObservationText.Length > 0));

                        IEditingItemViewModel itemToEdit = new EditingItemViewModel(
                          id: item.Id,
                          itemName: successData.ItemName,
                          itemPath: item.ItemPath,
                          body: new EditingBodyViewModel(
                              lawList: successData.LawList.ToViewLawList(),
                              form: new(successData.FormData.ToViewForm(rules.Data, observations.SourceItems)
                              .Append(observations)
                              .Append(new ImageContainerFormItemViewModel(
                           imageItems: new(
                  successData.Images
                          .Select(x => new ImageViewModel(id: x.Id, imagePath: x.ImagePath, imageObservation: x.ImageObservation))), "Imagens")))));
                        EditingItems.Add(item: itemToEdit);

                    }
                });
            }

            else
            {
                if (EditingItems.FirstOrDefault(predicate: x => !x.Id.Equals(value: item.Id)) is { } editingItem)
                {
                    SelectedItem = editingItem;
                    this.RaisePropertyChanged(propertyName: nameof(SelectedItem));
                }
            }
        });
}

public static class Extension
{
    public static ObservableCollection<ILawListViewModel> ToViewLawList(this IEnumerable<AppLawModel> lawModels) =>
        new(collection: lawModels.Select(selector: item =>
            new LawListViewModel(lawId: item.LawId, lawContent: item.LawTextContent)));

    public static ObservableCollection<IFormViewModel> ToViewForm(
        this IEnumerable<IAppFormDataItemContract> formItems,
        IEnumerable<ValidationRule> rules, SourceList<ObservationModel> observations) =>
        new(collection:
            formItems
            .Select<IAppFormDataItemContract, IFormViewModel>(selector: item =>
        {
            return item switch
            {
                AppFormDataItemTextModel text => new TextFormItemViewModel(
                    id: text.Id,
                    topic: text.Topic,
                textData: text.TextData,
                    measurementUnit: text.MeasurementUnit ?? "", observations: observations, rules: rules.Where(x => x.Targets.Any(i => i.Id == text.Id))),
                AppFormDataItemCheckboxModel checkbox => new CheckboxFormItem(id: checkbox.Id, topic: checkbox.Topic,
                    checkboxItems: new ObservableCollection<ICheckboxItemViewModel>(
                        collection: checkbox.Children.Select(
                            selector: child => new CheckboxItemViewModel(
                                id: child.Id,
                                topic: child.Topic,
                                observations: observations,
                                rules: rules.Where(x => x.Targets.Any(i => i.Id == child.Id)),
                                textItems: new ObservableCollection<ITextFormItemViewModel>(
                                    collection: child.TextItems.Select(selector: textItem =>
                                        new TextFormItemViewModel(id: textItem.Id,
                                                                  topic: textItem.Topic,
                                                                  textData: textItem.TextData,
                                                                  observations: observations,
                                                                  measurementUnit: textItem.MeasurementUnit ?? "",
                                                                  rules: rules.Where(x => x.Targets.Any(i => i.Id == child.Id))))),
                                options: new OptionContainerViewModel(
                                    options: new ObservableCollection<IOptionViewModel>(
                                        collection: child.Options.Select(selector: option =>
                                            new OptionItemViewModel(id: option.Id,
                                                                    value: option.Value,
                                                                    isChecked: option.IsChecked)))))))),
                _ => throw new ArgumentOutOfRangeException(paramName: nameof(item), actualValue: item, message: null)
            };
        }));

    public static AppItemModel ToAppModel(this IEditingBodyViewModel viewModel)
    {
        var appModel = new AppItemModel();

        appModel.FormData = viewModel.Form
            .Where(x => x is not IImageFormItemViewModel && x is not IObservationFormItemViewModel)
            .Select<IFormViewModel, IAppFormDataItemContract>(formData =>
        {
            return formData switch
            {
                TextFormItemViewModel text => new AppFormDataItemTextModel(id: text.Id, topic: text.Topic, textData: text.TextData,
                    measurementUnit: text.MeasurementUnit ?? ""),
                CheckboxFormItem checkbox => new AppFormDataItemCheckboxModel(id: checkbox.Id, topic: checkbox.Topic)
                {
                    Children = checkbox.CheckboxItems.Select(
                            selector: child => new AppFormDataItemCheckboxChildModel(id: child.Id, topic: child.Topic)
                            {
                                TextItems = child.TextItems.Select(selector: textItem =>
                                        new AppFormDataItemTextModel(
                                            id: textItem.Id,
                                            topic: textItem.Topic,
                                            textData: textItem.TextData,
                                            measurementUnit: textItem.MeasurementUnit ?? "")),
                                Options = child.Options.Options.Select(selector: option =>
                                            new AppOptionModel(
                                                id: option.Id,
                                                value: option.Value,
                                                isChecked: option.IsChecked))
                            })

                },
                _ => throw new ArgumentOutOfRangeException(paramName: nameof(formData), actualValue: formData, message: null)
            };
        });

        appModel.Images = viewModel
            .Form
            .Where(x => x is IImageFormItemViewModel)
            .Cast<IImageFormItemViewModel>()
            .SelectMany(x => x.ImageItems)
            .Select(x => new Core.Entities.Solution.Project.AppItem.DataItems.Images.ImagesItem() { Id = x.Id, ImagePath = x.ImagePath, ImageObservation = x.ImageObservation });

        var result = viewModel
            .Form
            .Where(x => x is IObservationFormItemViewModel)
            .Cast<IObservationFormItemViewModel>();

        appModel.Observations = result.SelectMany(x => x.Observations).Where(it => it.ObservationText.Length > 0);

        appModel.LawList = viewModel.LawList.Select(x => new AppLawModel(x.LawId, x.LawContent));

        return appModel;
    }
}