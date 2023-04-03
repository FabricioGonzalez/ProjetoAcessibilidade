using System.Linq;

using Core.Entities.Solution.Project.AppItem;
using Core.Entities.Solution.Project.AppItem.DataItems;
using Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using Core.Entities.Solution.Project.AppItem.DataItems.Images;
using Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using Core.Entities.Solution.Project.AppItem.DataItems.Text;

using DynamicData.Binding;

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

    [AutoNotify]
    private ObservableCollectionExtended<ReactiveObject> _formData;

    [AutoNotify]
    private ObservableCollectionExtended<LawStateItem> _lawItems;
}

public static class Extensions
{
    public static AppModelState ToAppState(this AppItemModel item)
    {
        return new AppModelState()
        {
            Id = item.Id,
            ItemName = item.ItemName,
            LawItems = new(item.LawList.Select(law =>
            new LawStateItem() { LawId = law.LawId, LawContent = law.LawTextContent })),
            FormData = new(
                item.FormData.Select<IAppFormDataItemContract, ReactiveObject>(item =>
{
    if (item is AppFormDataItemCheckboxModel checkbox)
    {
        return new CheckboxContainerItemState()
        {
            Topic = checkbox.Topic,
            Children = new(checkbox.Children
            .Select(checkboxItem =>
            {
                return new CheckboxItemState()
                {
                    Topic = checkboxItem.Topic,
                    TextItems = new(
                        checkboxItem.TextItems.Select(textItem =>
                        {
                            return new TextItemState()
                            {
                                Topic = textItem.Topic,
                                Type = textItem.Type,
                                MeasurementUnit = textItem.MeasurementUnit,
                                TextData = textItem.TextData,
                            };
                        })),
                    Options = new(
                        checkboxItem.Options
                        .Select(opt => new OptionsItemState() { IsChecked = opt.IsChecked, Value = opt.Value }))
                };
            }))
        };
    }

    if (item is AppFormDataItemTextModel text)
    {
        return new TextItemState()
        {
            Topic = text.Topic,
            Type = text.Type,
            MeasurementUnit = text.MeasurementUnit ?? "",
            TextData = text.TextData,
        };
    }

    if (item is AppFormDataItemObservationModel observation)
    {
        return new ObservationItemState()
        {
            Topic = observation.Topic,
            Observation = observation.Observation
        };
    }

    if (item is AppFormDataItemImageModel images)
    {
        return new ImageContainerItemState()
        {
            Topic = images.Topic,
            Type = images.Type,
            ImagesItems = new(
                images
            .ImagesItems
            .Select(image =>
            new ImageItemState()
            {
                ImagePath = image.ImagePath,
                ImageObservation = image.ImageObservation
            }))
        };
    }
    return null;
})),
        };
    }

    public static AppItemModel ToAppModel(this AppModelState item)
    {
        return new()
        {
            ItemName = item.ItemName,
            FormData = item.FormData.Select<ReactiveObject, IAppFormDataItemContract>(item =>
            {
                if (item is CheckboxContainerItemState checkbox)
                {
                    return new AppFormDataItemCheckboxModel(id: "", topic: checkbox.Topic, type: checkbox.Type)
                    {
                        Children = checkbox.Children.Select(item => new AppFormDataItemCheckboxChildModel(id: "", topic: item.Topic)
                        {
                            Options = item
                             .Options
                             .Select(item =>
                             new AppOptionModel(value: item.Value, isChecked: item.IsChecked, id: ""))
                             .ToList(),
                            TextItems =
                            item
                            .TextItems
                            .Select(item =>
                            new AppFormDataItemTextModel(
                                id: "",
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
                        id: "",
                            topic: text.Topic,
                            type: text.Type,
                            textData: text.TextData, measurementUnit: text.MeasurementUnit);
                }
                if (item is ImageContainerItemState images)
                {
                    return new AppFormDataItemImageModel(id: "", topic: images.Topic, type: images.Type)
                    {

                        ImagesItems = images.ImagesItems.Select(item =>
                        new ImagesItem(
                            id: "",
                        imagePath: item.ImagePath,
                        imageObservation: item.ImageObservation)).ToList(),
                    };
                }
                if (item is ObservationItemState observation)
                {
                    return new AppFormDataItemObservationModel(
                        id: "",
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
