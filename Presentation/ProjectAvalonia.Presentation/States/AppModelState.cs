using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.Presentation.States.LawItemState;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Images;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Text;
using ProjetoAcessibilidade.Core.Enuns;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public partial class AppModelState : ReactiveObject
{
    private ReadOnlyObservableCollection<FormItemStateBase> _formData;
    public ReadOnlyObservableCollection<FormItemStateBase> FormData
    {
        get => _formData;
        set => this.RaiseAndSetIfChanged(ref _formData,value);
    }

    private string _itemName;

    public string ItemName
    {
        get => _itemName;
        set => this.RaiseAndSetIfChanged(ref _itemName, value);
    }
    private string _itemTemplate;

    public string ItemTemplate
    {
        get => _itemTemplate;
        set => this.RaiseAndSetIfChanged(ref _itemTemplate, value);
    }

    private ReadOnlyObservableCollection<LawStateItem> _lawItems;
    public ReadOnlyObservableCollection<LawStateItem> LawItems
    {
        get => _lawItems;
        set => this.RaiseAndSetIfChanged(ref _lawItems, value);
    }
    private readonly SourceList<FormItemStateBase> formDataAggregate = new();


    private string _id;

    public string Id
    {
        get =>_id;
        set => this.RaiseAndSetIfChanged(ref _id,value);
    }
    private readonly SourceList<LawStateItem> lawItemsAggregate = new();

    public AppModelState()
    {
        formDataAggregate
            .Connect()
            .Bind(readOnlyObservableCollection: out _formData)
            .Subscribe();

        lawItemsAggregate
            .Connect()
            .Bind(readOnlyObservableCollection: out _lawItems)
            .Subscribe();
    }

    public void AddFormItem(
        FormItemStateBase formItem
    ) => formDataAggregate.Add(item: formItem);

    public void LoadItemData(
        IEnumerable<FormItemStateBase> items
    ) => formDataAggregate.AddRange(items: items);

    public void RemoveItem(
        FormItemStateBase formItem
    ) => formDataAggregate.Remove(item: formItem);

    public void ChangeItemType(
        FormItemStateBase formItemToReplace
        , FormItemStateBase newItem
    ) =>
        formDataAggregate.Replace(
            original: formItemToReplace,
            destination: newItem);

    public void AddLawItems(
        LawStateItem lawItem
    ) => lawItemsAggregate.Add(item: lawItem);

    public void LoadLawItems(
        IEnumerable<LawStateItem> items
    ) => lawItemsAggregate.AddRange(items: items);

    public void RemoveLawItem(
        LawStateItem lawItem
    ) => lawItemsAggregate.Remove(item: lawItem);

    public IObservable<IChangeSet<FormItemStateBase>> GetFormDataObservable() => formDataAggregate.Connect();
}
public static partial class Extensions
{
    public static AppModelState ToAppState(
        this AppItemModel item
    )
    {
        var toReturn = new AppModelState
        {
            Id = item.Id, ItemName = item.ItemName
        };
        toReturn.LoadItemData(
            items: item
                .FormData
                .Select<IAppFormDataItemContract, FormItemStateBase>(selector: item =>
                {
                    if (item is AppFormDataItemCheckboxModel checkbox)
                    {
                        return new CheckboxContainerItemState(topic: checkbox.Topic)
                        {
                            Id = checkbox.Id, Children = new ObservableCollectionExtended<CheckboxItemState>(
                                collection: checkbox.Children
                                    .Select(selector: checkboxItem =>
                                    {
                                        return new CheckboxItemState
                                        {
                                            Id = checkboxItem.Id, Topic = checkboxItem.Topic, TextItems =
                                                new ObservableCollectionExtended<TextItemState>(
                                                    collection: checkboxItem.TextItems.Select(selector: textItem =>
                                                    {
                                                        return new TextItemState(
                                                            id: textItem.Id,
                                                            topic: textItem.Topic,
                                                            textData: textItem.TextData,
                                                            measurementUnit: textItem.MeasurementUnit);
                                                    }))
                                            , Options = new ObservableCollectionExtended<OptionsItemState>(
                                                collection: checkboxItem.Options
                                                    .Select(selector: opt => new OptionsItemState
                                                    {
                                                        Id = opt.Id, IsChecked = opt.IsChecked, Value = opt.Value
                                                    }))
                                        };
                                    }))
                        };
                    }

                    if (item is AppFormDataItemTextModel text)
                    {
                        return new TextItemState(
                            id: text.Id,
                            topic: text.Topic,
                            textData: text.TextData,
                            measurementUnit: text.MeasurementUnit);
                    }

                    if (item is AppFormDataItemObservationModel observation)
                    {
                        return new ObservationItemState(
                            id: observation.Id,
                            topic: observation.Topic,
                            observation: observation.Observation);
                    }

                    if (item is AppFormDataItemImageModel images)
                    {
                        return new ImageContainerItemState
                        {
                            Id = images.Id, Topic = images.Topic, Type = images.Type, ImagesItems =
                                new ObservableCollectionExtended<ImageItemState>(
                                    collection: images
                                        .ImagesItems
                                        .Select(selector: image =>
                                            new ImageItemState
                                            {
                                                Id = image.Id, ImagePath = image.ImagePath
                                                , ImageObservation = image.ImageObservation
                                            }))
                        };
                    }

                    return null;
                }));

        toReturn.LoadLawItems(items: item
            .LawList
            .Select(selector: law =>
                new LawStateItem { LawId = law.LawId, LawContent = law.LawTextContent }));

        return toReturn;
    }

    public static AppItemModel ToAppModel(
        this AppModelState item
    ) =>
        new()
        {
            Id = !string.IsNullOrWhiteSpace(value: item.Id) ? item.Id : Guid.NewGuid().ToString()
            , ItemName = item.ItemName, FormData = item.FormData.Select<FormItemStateBase, IAppFormDataItemContract>(
                selector: item =>
                {
                    if (item is CheckboxContainerItemState checkbox)
                    {
                        return new AppFormDataItemCheckboxModel(
                            id: !string.IsNullOrWhiteSpace(value: checkbox.Id)
                                ? item.Id
                                : Guid.NewGuid().ToString(),
                            topic: checkbox.Topic,
                            type: checkbox.Type)
                        {
                            Children = checkbox
                                .Children
                                .Select(selector: item => new AppFormDataItemCheckboxChildModel(
                                    id: !string.IsNullOrWhiteSpace(value: item.Id)
                                        ? item.Id
                                        : Guid.NewGuid().ToString(),
                                    topic: item.Topic)
                                {
                                    Options = item
                                        .Options
                                        .Select(selector: item =>
                                            new AppOptionModel(
                                                value: item.Value,
                                                isChecked: item.IsChecked,
                                                id: !string.IsNullOrWhiteSpace(value: item.Id)
                                                    ? item.Id
                                                    : Guid.NewGuid().ToString())
                                        )
                                        .ToList()
                                    , TextItems =
                                        item
                                            .TextItems
                                            .Select(selector: item =>
                                                new AppFormDataItemTextModel(
                                                    id: !string.IsNullOrWhiteSpace(value: item.Id)
                                                        ? item.Id
                                                        : Guid.NewGuid().ToString(),
                                                    topic: item.Topic,
                                                    type: item.Type,
                                                    textData: item.TextData,
                                                    measurementUnit: item.MeasurementUnit))
                                            .ToList()
                                })
                                .ToList()
                        };
                    }

                    if (item is TextItemState text)
                    {
                        return new AppFormDataItemTextModel(
                            id: !string.IsNullOrWhiteSpace(value: item.Id)
                                ? item.Id
                                : Guid.NewGuid().ToString(),
                            topic: text.Topic,
                            type: text.Type,
                            textData: text.TextData, measurementUnit: text.MeasurementUnit);
                    }

                    if (item is ImageContainerItemState images)
                    {
                        return new AppFormDataItemImageModel(id:
                            !string.IsNullOrWhiteSpace(value: item.Id)
                                ? item.Id
                                : Guid.NewGuid().ToString(), topic: images.Topic, type: images.Type)
                        {
                            ImagesItems = images.ImagesItems.Select(selector: item =>
                                new ImagesItem(
                                    id: !string.IsNullOrWhiteSpace(value: item.Id)
                                        ? item.Id
                                        : Guid.NewGuid().ToString(),
                                    imagePath: item.ImagePath,
                                    imageObservation: item.ImageObservation)).ToList()
                        };
                    }

                    if (item is ObservationItemState observation)
                    {
                        return new AppFormDataItemObservationModel(
                            id: !string.IsNullOrWhiteSpace(value: item.Id)
                                ? item.Id
                                : Guid.NewGuid().ToString(),
                            type: AppFormDataType.Observação,
                            observation: observation.Observation,
                            topic: observation.Topic);
                    }

                    return null;
                }).ToList()
            , LawList = item
                .LawItems
                .Select(selector: x =>
                    new AppLawModel(
                        lawId: x.LawId,
                        lawTextContent: x.LawContent))
                .ToList()
        };
}