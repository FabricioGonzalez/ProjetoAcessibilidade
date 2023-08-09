using System.Collections.ObjectModel;
using DynamicData;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.Presentation.States.LawItemState;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public class AppModelState : ReactiveObject
{
    private readonly SourceList<FormItemContainer> formDataAggregate = new();
    private readonly SourceList<LawStateItem> lawItemsAggregate = new();

    private ObservableCollection<FormItemContainer> _formData;
    private string _id;

    private string _itemName;
    private string _itemTemplate;
    private ObservableCollection<LawStateItem> _lawItems;

    public AppModelState()
    {
        _formData = new ObservableCollection<FormItemContainer>();
        _lawItems = new ObservableCollection<LawStateItem>();
    }

    public ObservableCollection<FormItemContainer> FormData
    {
        get => _formData;
        set => this.RaiseAndSetIfChanged(
            backingField: ref _formData,
            newValue: value);
    }

    public string ItemName
    {
        get => _itemName;
        set => this.RaiseAndSetIfChanged(
            backingField: ref _itemName,
            newValue: value);
    }

    public string ItemTemplate
    {
        get => _itemTemplate;
        set => this.RaiseAndSetIfChanged(
            backingField: ref _itemTemplate,
            newValue: value);
    }

    public ObservableCollection<LawStateItem> LawItems
    {
        get => _lawItems;
        set => this.RaiseAndSetIfChanged(
            backingField: ref _lawItems,
            newValue: value);
    }

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(
            backingField: ref _id,
            newValue: value);
    }

    public void AddFormItem(
        FormItemContainer formItem
    ) =>
        /*formDataAggregate.Add(formItem);*/
        FormData.Add(formItem);

    public void LoadItemData(
        IEnumerable<FormItemContainer>? items
    ) =>
        // formDataAggregate.AddRange(items);
        FormData.AddRange(items);

    public void RemoveItem(
        FormItemContainer formItem
    ) =>
        /*formDataAggregate.Remove(formItem);*/
        _ = FormData.Remove(formItem);


    public void ChangeItemType(
        FormItemContainer formItemToReplace
        , FormItemContainer newItem
    ) =>
        formDataAggregate.Replace(
            original: formItemToReplace,
            destination: newItem);

    public void AddLawItems(
        LawStateItem lawItem
    ) =>
        /*lawItemsAggregate.Add(lawItem);*/
        LawItems.Add(lawItem);


    public void LoadLawItems(
        IEnumerable<LawStateItem> items
    ) =>
        /*lawItemsAggregate.AddRange(items);*/
        LawItems.AddRange(items);

    public void RemoveLawItem(
        LawStateItem lawItem
    ) =>
        /*lawItemsAggregate.Remove(lawItem);*/
        _ = LawItems.Remove(lawItem);


    public IObservable<IChangeSet<FormItemContainer>> GetFormDataObservable() => formDataAggregate.Connect();
}

/*public static class Extensions
{
    public static AppModelState ToAppStateFillable(
        this AppItemModel item
    )
    {
        var toReturn = new AppModelState
        {
            Id = item.Id, ItemName = item.ItemName, ItemTemplate = item.ItemName
        };
        toReturn.LoadItemData(
            item
                ?.FormData
                ?.Select(
                    item =>
                    {
                        if (item is AppFormDataItemCheckboxModel checkbox)
                        {
                            return new FormItemContainer
                            {
                                Type = checkbox.Type, Topic = checkbox.Topic, Body =
                                    new CheckboxContainerItemState(checkbox.Topic)
                                    {
                                        Id = checkbox.Id, Children =
                                            new ObservableCollectionExtended<CheckboxItemState>(
                                                checkbox.Children
                                                    .Select(
                                                        checkboxItem =>
                                                        {
                                                            return new CheckboxItemState
                                                            {
                                                                Id = checkboxItem.Id, Topic = checkboxItem.Topic
                                                                , TextItems =
                                                                    new ObservableCollectionExtended<TextItemState>(
                                                                        checkboxItem.TextItems.Select(
                                                                            textItem =>
                                                                            {
                                                                                return new TextItemState(
                                                                                    id: textItem.Id,
                                                                                    topic: textItem.Topic,
                                                                                    textData: textItem.TextData,
                                                                                    measurementUnit: textItem
                                                                                        .MeasurementUnit);
                                                                            }))
                                                                , Options =
                                                                    new ObservableCollectionExtended<OptionsItemState>(
                                                                        checkboxItem.Options
                                                                            .Select(
                                                                                opt => new OptionsItemState
                                                                                {
                                                                                    Id = opt.Id
                                                                                    , IsChecked = opt.IsChecked
                                                                                    , Value = opt.Value
                                                                                }))
                                                            };
                                                        }))
                                    }
                            };
                        }

                        if (item is AppFormDataItemTextModel text)
                        {
                            return new FormItemContainer
                            {
                                Type = text.Type, Topic = text.Topic, Body = new TextItemState(
                                    id: text.Id,
                                    topic: text.Topic,
                                    textData: text.TextData,
                                    measurementUnit: text.MeasurementUnit)
                            };
                        }

                        return null;
                    }).Where(val => val != null) ?? Enumerable.Empty<FormItemContainer>());

        toReturn.LoadLawItems(
            item
                .LawList
                .Select(
                    law =>
                        new LawStateItem { LawId = law.LawId, LawContent = law.LawTextContent }));

        return toReturn;
    }

    public static AppModelState ToAppState(
        this AppItemModel item
    )
    {
        var toReturn = new AppModelState
        {
            Id = item.Id, ItemName = item.ItemName, ItemTemplate = item.ItemName
        };
        toReturn.LoadItemData(
            item
                ?.FormData
                ?.Select(
                    item =>
                    {
                        if (item is AppFormDataItemCheckboxModel checkbox)
                        {
                            return new FormItemContainer
                            {
                                Topic = checkbox.Topic, Body = new CheckboxContainerItemState(checkbox.Topic)
                                {
                                    Id = checkbox.Id, Children = new ObservableCollectionExtended<CheckboxItemState>(
                                        checkbox.Children
                                            .Select(
                                                checkboxItem =>
                                                {
                                                    return new CheckboxItemState
                                                    {
                                                        Id = checkboxItem.Id, Topic = checkboxItem.Topic, TextItems =
                                                            new ObservableCollectionExtended<TextItemState>(
                                                                checkboxItem.TextItems.Select(
                                                                    textItem =>
                                                                    {
                                                                        return new TextItemState(
                                                                            id: textItem.Id,
                                                                            topic: textItem.Topic,
                                                                            textData: textItem.TextData,
                                                                            measurementUnit: textItem.MeasurementUnit);
                                                                    }))
                                                        , Options = new ObservableCollectionExtended<OptionsItemState>(
                                                            checkboxItem.Options
                                                                .Select(
                                                                    opt => new OptionsItemState
                                                                    {
                                                                        Id = opt.Id, IsChecked = opt.IsChecked
                                                                        , Value = opt.Value
                                                                    }))
                                                    };
                                                }))
                                }
                            };
                        }

                        if (item is AppFormDataItemTextModel text)
                        {
                            return new FormItemContainer
                            {
                                Topic = text.Topic, Body = new TextItemState(
                                    id: text.Id,
                                    topic: text.Topic,
                                    textData: text.TextData,
                                    measurementUnit: text.MeasurementUnit)
                            };
                        }

                        if (item is AppFormDataItemObservationModel observation)
                        {
                            return new FormItemContainer
                            {
                                Topic = observation.Topic, Body = new ObservationItemState(
                                    id: observation.Id,
                                    topic: observation.Topic,
                                    observation: observation.Observation)
                            };
                        }

                        if (item is AppFormDataItemImageModel images)
                        {
                            return new FormItemContainer
                            {
                                Topic = images.Topic, Body = new ImageContainerItemState
                                {
                                    Id = images.Id, Topic = images.Topic, Type = images.Type, ImagesItems =
                                        new ObservableCollectionExtended<ImageItemState>(
                                            images
                                                .ImagesItems
                                                .Select(
                                                    image =>
                                                        new ImageItemState
                                                        {
                                                            Id = image.Id, ImagePath = image.ImagePath
                                                            , ImageObservation = image.ImageObservation
                                                        }))
                                }
                            };
                        }

                        return null;
                    }) ?? Enumerable.Empty<FormItemContainer>());

        toReturn.LoadLawItems(
            item
                .LawList
                .Select(
                    law =>
                        new LawStateItem { LawId = law.LawId, LawContent = law.LawTextContent }));

        return toReturn;
    }

    public static AppItemModel ToAppModel(
        this AppModelState item
    ) =>
        new()
        {
            Id = !string.IsNullOrWhiteSpace(item.Id)
                ? item.Id
                : Guid.NewGuid()
                    .ToString()
            , ItemName = item.ItemName, FormData = item.FormData.Select<FormItemContainer, IAppFormDataItemContract>(
                    formItem =>
                    {
                        if (formItem.Body is CheckboxContainerItemState checkbox)
                        {
                            return new AppFormDataItemCheckboxModel(
                                !string.IsNullOrWhiteSpace(checkbox.Id)
                                    ? checkbox.Id
                                    : Guid.NewGuid()
                                        .ToString(),
                                checkbox.Topic,
                                checkbox.Type)
                            {
                                Children = checkbox
                                    .Children
                                    .Select(
                                        checkboxChild => new AppFormDataItemCheckboxChildModel(
                                            !string.IsNullOrWhiteSpace(checkboxChild.Id)
                                                ? checkboxChild.Id
                                                : Guid.NewGuid()
                                                    .ToString(),
                                            checkboxChild.Topic,
                                            false)
                                        {
                                            Options = checkboxChild
                                                .Options
                                                .Select(
                                                    option =>
                                                        new AppOptionModel(
                                                            option.Value,
                                                            isChecked: option.IsChecked,
                                                            id: !string.IsNullOrWhiteSpace(option.Id)
                                                                ? option.Id
                                                                : Guid.NewGuid()
                                                                    .ToString())
                                                )
                                                .ToList()
                                            , TextItems =
                                                checkboxChild
                                                    .TextItems
                                                    .Select(
                                                        textItem =>
                                                            new AppFormDataItemTextModel(
                                                                !string.IsNullOrWhiteSpace(textItem.Id)
                                                                    ? textItem.Id
                                                                    : Guid.NewGuid()
                                                                        .ToString(),
                                                                textItem.Topic,
                                                                textItem.Type,
                                                                textItem.TextData,
                                                                textItem.MeasurementUnit))
                                                    .ToList()
                                        })
                                    .ToList()
                            };
                        }

                        if (formItem.Body is TextItemState text)
                        {
                            return new AppFormDataItemTextModel(
                                !string.IsNullOrWhiteSpace(text.Id)
                                    ? text.Id
                                    : Guid.NewGuid()
                                        .ToString(),
                                text.Topic,
                                text.Type,
                                text.TextData,
                                text.MeasurementUnit);
                        }

                        if (formItem.Body is ImageContainerItemState images)
                        {
                            return new AppFormDataItemImageModel(
                                !string.IsNullOrWhiteSpace(images.Id)
                                    ? images.Id
                                    : Guid.NewGuid()
                                        .ToString(),
                                images.Topic,
                                images.Type)
                            {
                                ImagesItems = images.ImagesItems.Select(
                                        image =>
                                            new ImagesItem(
                                                !string.IsNullOrWhiteSpace(image.Id)
                                                    ? image.Id
                                                    : Guid.NewGuid()
                                                        .ToString(),
                                                image.ImagePath,
                                                image.ImageObservation))
                                    .ToList()
                            };
                        }

                        if (formItem.Body is ObservationItemState observation)
                        {
                            return new AppFormDataItemObservationModel(
                                id: !string.IsNullOrWhiteSpace(formItem.Id)
                                    ? formItem.Id
                                    : Guid.NewGuid()
                                        .ToString(),
                                type: AppFormDataType.Observação,
                                observation: observation.Observation,
                                topic: observation.Topic);
                        }

                        return null;
                    })
                .ToList()
            , Images = Enumerable.Empty<ImagesItem>(), Observations = Enumerable.Empty<ObservationModel>()
            , TemplateName = item.ItemName, LawList = item
                .LawItems
                .Select(
                    x =>
                        new AppLawModel(
                            x.LawId,
                            x.LawContent))
                .ToList()
        };
}*/