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
                ImagePath = image.imagePath,
                ImageObservation = image.imageObservation
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
                    return new AppFormDataItemCheckboxModel()
                    {
                        Topic = checkbox.Topic,
                        Type = checkbox.Type,
                        Children = checkbox.Children.Select(item => new AppFormDataItemCheckboxChildModel()
                        {
                            Topic = item.Topic,
                            Options = item
                             .Options
                             .Select(item =>
                             new AppOptionModel()
                             {
                                 IsChecked = item.IsChecked,
                                 Value = item.Value
                             })
                             .ToList(),
                            TextItems =
                            item
                            .TextItems
                            .Select(item => new AppFormDataItemTextModel()
                            {
                                Topic = item.Topic,
                                MeasurementUnit = item.MeasurementUnit,
                                TextData = item.TextData,
                                Type = item.Type
                            })
                            .ToList()
                        })
                        .ToList(),
                    };
                }
                if (item is TextItemState text)
                {
                    return new AppFormDataItemTextModel()
                    {
                        Topic = text.Topic,
                        MeasurementUnit = text.MeasurementUnit,
                        TextData = text.TextData,
                        Type = text.Type
                    };
                }
                if (item is ImageContainerItemState images)
                {
                    return new AppFormDataItemImageModel()
                    {
                        Topic = images.Topic,
                        ImagesItems = images.ImagesItems.Select(item => new ImagesItem()
                        {
                            imageObservation = item.ImageObservation,
                            imagePath = item.ImagePath
                        }).ToList(),
                        Type = images.Type
                    };
                }
                if (item is ObservationItemState observation)
                {
                    return new AppFormDataItemObservationModel()
                    {
                        Topic = observation.Topic,
                        Observation = observation.Observation,
                        Type = Core.Enuns.AppFormDataType.Observação
                    };
                }
                return null;

            }).ToList(),
            LawList = item
                    .LawItems
                    .Select(x => new AppLawModel()
                    {
                        LawId = x.LawId,
                        LawTextContent = x.LawContent
                    })
                    .ToList()
        };
    }
}
