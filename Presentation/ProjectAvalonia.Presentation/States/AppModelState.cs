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
        _formData = new();
        _lawItems = new();
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
    ) => formDataAggregate.Add(formItem);

    public void LoadItemData(
        IEnumerable<FormItemContainer>? items
    )
    {
        // formDataAggregate.AddRange(items);
        FormData.AddRange(items);
    }

    public void RemoveItem(
        FormItemContainer formItem
    ) => formDataAggregate.Remove(formItem);

    public void ChangeItemType(
        FormItemContainer formItemToReplace,
        FormItemContainer newItem
    ) =>
        formDataAggregate.Replace(
            original: formItemToReplace,
            destination: newItem);

    public void AddLawItems(
        LawStateItem lawItem
    ) => lawItemsAggregate.Add(lawItem);

    public void LoadLawItems(
        IEnumerable<LawStateItem> items
    )
    {
        /*lawItemsAggregate.AddRange(items);*/
        LawItems.AddRange(items);
    }

    public void RemoveLawItem(
        LawStateItem lawItem
    ) => lawItemsAggregate.Remove(lawItem);

    public IObservable<IChangeSet<FormItemContainer>> GetFormDataObservable() => formDataAggregate.Connect();
}

public static class Extensions
{
    public static AppModelState ToAppStateFillable(
        this AppItemModel item
    )
    {
        var toReturn = new AppModelState
        {
            Id = item.Id,
            ItemName = item.ItemName,
            ItemTemplate = item.ItemName
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
                                Type = checkbox.Type,
                                Topic = checkbox.Topic,
                                Body = new CheckboxContainerItemState(checkbox.Topic)
                                {
                                    Id = checkbox.Id,
                                    Children = new ObservableCollectionExtended<CheckboxItemState>(
                                        checkbox.Children
                                            .Select(
                                                checkboxItem =>
                                                {
                                                    return new CheckboxItemState
                                                    {
                                                        Id = checkboxItem.Id,
                                                        Topic = checkboxItem.Topic,
                                                        TextItems =
                                                            new ObservableCollectionExtended<TextItemState>(
                                                                checkboxItem.TextItems.Select(
                                                                    textItem =>
                                                                    {
                                                                        return new TextItemState(
                                                                            id: textItem.Id,
                                                                            topic: textItem.Topic,
                                                                            textData: textItem.TextData,
                                                                            measurementUnit: textItem.MeasurementUnit);
                                                                    })),
                                                        Options = new ObservableCollectionExtended<OptionsItemState>(
                                                            checkboxItem.Options
                                                                .Select(
                                                                    opt => new OptionsItemState
                                                                    {
                                                                        Id = opt.Id,
                                                                        IsChecked = opt.IsChecked,
                                                                        Value = opt.Value
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
                                Type = text.Type,
                                Topic = text.Topic,
                                Body = new TextItemState(
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
            Id = item.Id,
            ItemName = item.ItemName,
            ItemTemplate = item.ItemName
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
                                Topic = checkbox.Topic,
                                Body = new CheckboxContainerItemState(checkbox.Topic)
                                {
                                    Id = checkbox.Id,
                                    Children = new ObservableCollectionExtended<CheckboxItemState>(
                                        checkbox.Children
                                            .Select(
                                                checkboxItem =>
                                                {
                                                    return new CheckboxItemState
                                                    {
                                                        Id = checkboxItem.Id,
                                                        Topic = checkboxItem.Topic,
                                                        TextItems =
                                                            new ObservableCollectionExtended<TextItemState>(
                                                                checkboxItem.TextItems.Select(
                                                                    textItem =>
                                                                    {
                                                                        return new TextItemState(
                                                                            id: textItem.Id,
                                                                            topic: textItem.Topic,
                                                                            textData: textItem.TextData,
                                                                            measurementUnit: textItem.MeasurementUnit);
                                                                    })),
                                                        Options = new ObservableCollectionExtended<OptionsItemState>(
                                                            checkboxItem.Options
                                                                .Select(
                                                                    opt => new OptionsItemState
                                                                    {
                                                                        Id = opt.Id,
                                                                        IsChecked = opt.IsChecked,
                                                                        Value = opt.Value
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
                                Topic = text.Topic,
                                Body = new TextItemState(
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
                                Topic = observation.Topic,
                                Body = new ObservationItemState(
                                    id: observation.Id,
                                    topic: observation.Topic,
                                    observation: observation.Observation)
                            };
                        }

                        if (item is AppFormDataItemImageModel images)
                        {
                            return new FormItemContainer
                            {
                                Topic = images.Topic,
                                Body = new ImageContainerItemState
                                {
                                    Id = images.Id,
                                    Topic = images.Topic,
                                    Type = images.Type,
                                    ImagesItems =
                                        new ObservableCollectionExtended<ImageItemState>(
                                            images
                                                .ImagesItems
                                                .Select(
                                                    image =>
                                                        new ImageItemState
                                                        {
                                                            Id = image.Id,
                                                            ImagePath = image.ImagePath,
                                                            ImageObservation = image.ImageObservation
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
                    .ToString(),
            ItemName = item.ItemName,
            FormData = item.FormData.Select<FormItemContainer, IAppFormDataItemContract>(
                    item =>
                    {
                        if (item.Body is CheckboxContainerItemState checkbox)
                        {
                            return new AppFormDataItemCheckboxModel(
                                id: !string.IsNullOrWhiteSpace(checkbox.Id)
                                    ? item.Id
                                    : Guid.NewGuid()
                                        .ToString(),
                                topic: checkbox.Topic,
                                type: checkbox.Type)
                            {
                                Children = checkbox
                                    .Children
                                    .Select(
                                        item => new AppFormDataItemCheckboxChildModel(
                                            id: !string.IsNullOrWhiteSpace(item.Id)
                                                ? item.Id
                                                : Guid.NewGuid()
                                                    .ToString(),
                                            topic: item.Topic)
                                        {
                                            Options = item
                                                .Options
                                                .Select(
                                                    item =>
                                                        new AppOptionModel(
                                                            value: item.Value,
                                                            isChecked: item.IsChecked,
                                                            id: !string.IsNullOrWhiteSpace(item.Id)
                                                                ? item.Id
                                                                : Guid.NewGuid()
                                                                    .ToString())
                                                )
                                                .ToList(),
                                            TextItems =
                                                item
                                                    .TextItems
                                                    .Select(
                                                        item =>
                                                            new AppFormDataItemTextModel(
                                                                id: !string.IsNullOrWhiteSpace(item.Id)
                                                                    ? item.Id
                                                                    : Guid.NewGuid()
                                                                        .ToString(),
                                                                topic: item.Topic,
                                                                type: item.Type,
                                                                textData: item.TextData,
                                                                measurementUnit: item.MeasurementUnit))
                                                    .ToList()
                                        })
                                    .ToList()
                            };
                        }

                        if (item.Body is TextItemState text)
                        {
                            return new AppFormDataItemTextModel(
                                id: !string.IsNullOrWhiteSpace(item.Id)
                                    ? item.Id
                                    : Guid.NewGuid()
                                        .ToString(),
                                topic: text.Topic,
                                type: text.Type,
                                textData: text.TextData,
                                measurementUnit: text.MeasurementUnit);
                        }

                        if (item.Body is ImageContainerItemState images)
                        {
                            return new AppFormDataItemImageModel(
                                id: !string.IsNullOrWhiteSpace(item.Id)
                                    ? item.Id
                                    : Guid.NewGuid()
                                        .ToString(),
                                topic: images.Topic,
                                type: images.Type)
                            {
                                ImagesItems = images.ImagesItems.Select(
                                        item =>
                                            new ImagesItem(
                                                id: !string.IsNullOrWhiteSpace(item.Id)
                                                    ? item.Id
                                                    : Guid.NewGuid()
                                                        .ToString(),
                                                imagePath: item.ImagePath,
                                                imageObservation: item.ImageObservation))
                                    .ToList()
                            };
                        }

                        if (item.Body is ObservationItemState observation)
                        {
                            return new AppFormDataItemObservationModel(
                                id: !string.IsNullOrWhiteSpace(item.Id)
                                    ? item.Id
                                    : Guid.NewGuid()
                                        .ToString(),
                                type: AppFormDataType.Observação,
                                observation: observation.Observation,
                                topic: observation.Topic);
                        }

                        return null;
                    })
                .ToList(),
            LawList = item
                .LawItems
                .Select(
                    x =>
                        new AppLawModel(
                            lawId: x.LawId,
                            lawTextContent: x.LawContent))
                .ToList()
        };
}