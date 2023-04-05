using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using Core.Entities.Solution.Project.AppItem;
using Core.Entities.Solution.Project.AppItem.DataItems;
using Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using Core.Entities.Solution.Project.AppItem.DataItems.Images;
using Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using Core.Entities.Solution.Project.AppItem.DataItems.Text;

using DynamicData;

using ProjectAvalonia.Features.Project.States.FormItemState;
using ProjectAvalonia.Features.Project.States.LawItemState;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States;
public partial class AppModelState : ReactiveObject
{
    [AutoNotify]
    private string id;

    [AutoNotify]
    private string _itemName;

    [AutoNotify]
    private string _itemTemplate;

    private SourceList<FormItemStateBase> formDataAggregate = new();

    [AutoNotify]
    private ReadOnlyObservableCollection<FormItemStateBase> _formData;

    private SourceList<LawStateItem> lawItemsAggregate = new();

    [AutoNotify]
    private ReadOnlyObservableCollection<LawStateItem> _lawItems;
    public AppModelState()
    {
        formDataAggregate
            .Connect()
            .Bind(readOnlyObservableCollection: out _formData)
            .Subscribe();

        lawItemsAggregate
            .Connect()
            .Bind(out _lawItems)
            .Subscribe();
    }
    public void AddFormItem(FormItemStateBase formItem)
    {
        formDataAggregate.Add(item: formItem);
    }

    public void LoadItemData(IEnumerable<FormItemStateBase> items)
    {
        formDataAggregate.AddRange(items);
    }

    public void RemoveItem(FormItemStateBase formItem)
    {
        formDataAggregate.Remove(item: formItem);
    }

    public void ChangeItemType(FormItemStateBase formItemToReplace, FormItemStateBase newItem)
    {
        formDataAggregate.Replace(
            formItemToReplace,
            newItem);
    }

    public void AddLawItems(LawStateItem lawItem)
    {
        lawItemsAggregate.Add(item: lawItem);
    }

    public void LoadLawItems(IEnumerable<LawStateItem> items)
    {
        lawItemsAggregate.AddRange(items);
    }

    public void RemoveLawItem(LawStateItem lawItem)
    {
        lawItemsAggregate.Remove(item: lawItem);
    }

    public IObservable<IChangeSet<FormItemStateBase>> GetFormDataObservable()
    {
        return formDataAggregate.Connect();
    }

}

public static class Extensions
{
    public static AppModelState ToAppState(this AppItemModel item)
    {

        var toReturn = new AppModelState()
        {
            Id = item.Id,
            ItemName = item.ItemName,
        };
        toReturn.LoadItemData(
                items: item
                .FormData
                .Select<IAppFormDataItemContract, FormItemStateBase>(item =>
                {
                    if (item is AppFormDataItemCheckboxModel checkbox)
                    {
                        return new CheckboxContainerItemState(topic: checkbox.Topic)
                        {
                            Id = checkbox.Id,
                            Children = new(checkbox.Children
                            .Select(checkboxItem =>
                            {
                                return new CheckboxItemState()
                                {
                                    Id = checkboxItem.Id,
                                    Topic = checkboxItem.Topic,
                                    TextItems = new(
                                        checkboxItem.TextItems.Select(textItem =>
                                        {
                                            return new TextItemState(
                                                id: textItem.Id,
                                                topic: textItem.Topic,
                                                textData: textItem.TextData,
                                                measurementUnit: textItem.MeasurementUnit);
                                        })),
                                    Options = new(
                                        checkboxItem.Options
                                        .Select(opt => new OptionsItemState()
                                        {
                                            Id = opt.Id,
                                            IsChecked = opt.IsChecked,
                                            Value = opt.Value
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
                        return new ImageContainerItemState()
                        {
                            Id = images.Id,
                            Topic = images.Topic,
                            Type = images.Type,
                            ImagesItems = new(
                                images
                            .ImagesItems
                            .Select(image =>
                            new ImageItemState()
                            {
                                Id = image.Id,
                                ImagePath = image.ImagePath,
                                ImageObservation = image.ImageObservation
                            }))
                        };
                    }
                    return null;
                }));

        toReturn.LoadLawItems(item
            .LawList
            .Select(law =>
            new LawStateItem() { LawId = law.LawId, LawContent = law.LawTextContent }));

        return toReturn;
    }

    public static AppItemModel ToAppModel(this AppModelState item)
    {
        return new()
        {
            Id = !string.IsNullOrWhiteSpace(item.Id) ? item.Id : Guid.NewGuid().ToString(),
            ItemName = item.ItemName,
            FormData = item.FormData.Select<FormItemStateBase, IAppFormDataItemContract>(item =>
            {
                if (item is CheckboxContainerItemState checkbox)
                {
                    return new AppFormDataItemCheckboxModel(
                        id: !string.IsNullOrWhiteSpace(checkbox.Id)
                        ? item.Id
                        : Guid.NewGuid().ToString(),
                        topic: checkbox.Topic,
                        type: checkbox.Type)
                    {
                        Children = checkbox
                        .Children
                        .Select(item => new AppFormDataItemCheckboxChildModel(
                            id: !string.IsNullOrWhiteSpace(item.Id)
                            ? item.Id
                            : Guid.NewGuid().ToString(),
                            topic: item.Topic)
                        {
                            Options = item
                             .Options
                             .Select(item =>
                             new AppOptionModel(
                                 value: item.Value,
                                 isChecked: item.IsChecked,
                             id: !string.IsNullOrWhiteSpace(item.Id)
                            ? item.Id
                            : Guid.NewGuid().ToString())
                             )
                             .ToList(),
                            TextItems =
                            item
                            .TextItems
                            .Select(item =>
                            new AppFormDataItemTextModel(
                                id: !string.IsNullOrWhiteSpace(item.Id)
                            ? item.Id
                            : Guid.NewGuid().ToString(),
                            topic: item.Topic,
                            type: item.Type,
                            textData: item.TextData,
                            measurementUnit: item.MeasurementUnit))
                            .ToList()
                        })
                                .ToList(),
                    };
                }

                if (item is TextItemState text)
                {
                    return new AppFormDataItemTextModel(
                        id: !string.IsNullOrWhiteSpace(item.Id)
                            ? item.Id
                            : Guid.NewGuid().ToString(),
                            topic: text.Topic,
                            type: text.Type,
                            textData: text.TextData, measurementUnit: text.MeasurementUnit);
                }
                if (item is ImageContainerItemState images)
                {
                    return new AppFormDataItemImageModel(id:
                        !string.IsNullOrWhiteSpace(item.Id)
                            ? item.Id
                            : Guid.NewGuid().ToString(), topic: images.Topic, type: images.Type)
                    {

                        ImagesItems = images.ImagesItems.Select(item =>
                        new ImagesItem(
                             id: !string.IsNullOrWhiteSpace(item.Id)
                            ? item.Id
                            : Guid.NewGuid().ToString(),
                        imagePath: item.ImagePath,
                        imageObservation: item.ImageObservation)).ToList(),
                    };
                }
                if (item is ObservationItemState observation)
                {
                    return new AppFormDataItemObservationModel(
                        id: !string.IsNullOrWhiteSpace(item.Id)
                            ? item.Id
                            : Guid.NewGuid().ToString(),
                        type: Core.Enuns.AppFormDataType.Observação,
                        observation: observation.Observation,
                        topic: observation.Topic);
                }
                return null;

            }).ToList(),
            LawList = item
                    .LawItems
                    .Select(x =>
                    new AppLawModel(
                        lawId: x.LawId,
                        lawTextContent: x.LawContent))
                    .ToList()
        };
    }
}
