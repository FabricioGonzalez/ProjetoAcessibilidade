using System.Xml.Serialization;

using Core.Entities.Solution.Project.AppItem;
using Core.Entities.Solution.Project.AppItem.DataItems;
using Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using Core.Entities.Solution.Project.AppItem.DataItems.Images;
using Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using Core.Entities.Solution.Project.AppItem.DataItems.Text;

using ProjectItemReader.XmlFile.DTO.FormItem;

namespace ProjectItemReader.XmlFile.DTO;

[XmlRoot(elementName: "item")]
public class ItemRoot
{
    [XmlElement(elementName: "id")]
    public string Id
    {

        get; set;
    }
    [XmlElement(elementName: "name")]
    public string ItemName
    {
        get; set;
    }
    [XmlElement(elementName: "template_name")]
    public string TemplateName
    {
        get; set;
    }
    [XmlArray(elementName: "table")]
    [XmlArrayItem(elementName: "checkbox_items", Type = typeof(ItemFormDataCheckboxModel))]
    [XmlArrayItem(elementName: "text_item", Type = typeof(ItemFormDataTextModel))]
    [XmlArrayItem(elementName: "image_items", Type = typeof(ItemFormDataItemImageModel))]
    [XmlArrayItem(elementName: "observation_item", Type = typeof(ItemFormDataObservationModel))]

    public List<ItemFormDataContainer> FormData
    {
        get; set;
    }
    [XmlArray(elementName: "law")]
    [XmlArrayItem(elementName: "law_item", Type = typeof(ItemLaw))]
    public List<ItemLaw> LawList
    {
        get; set;
    }
}

public static partial class Extensions
{
    public static ItemRoot ToItemRoot(this AppItemModel model)
    {
        return new()
        {
            Id = model.Id,
            FormData = model.FormData.Select<IAppFormDataItemContract, ItemFormDataContainer>(item =>
            {
                if (item is AppFormDataItemCheckboxModel checkbox)
                {
                    return new ItemFormDataCheckboxModel(id: checkbox.Id, topic: checkbox.Topic)
                    {
                        Children = checkbox.Children.Select(item =>
                        {
                            return new ItemFormDataCheckboxChildModel(id: item.Id, topic: item.Topic)
                            {
                                Options = item.Options.Select(option =>
                                new ItemOptionModel(
                                    value: option.Value,
                                id: option.Id,
                                isChecked: option.IsChecked)).ToList(),
                            };
                        }).ToList()
                    };
                }

                if (item is AppFormDataItemTextModel text)
                {
                    return new ItemFormDataTextModel(
                        id: text.Id,
                        topic: text.Topic,
                        textData: text.TextData,
                        measurementUnit: text.MeasurementUnit);
                }

                if (item is AppFormDataItemImageModel images)
                {
                    return new ItemFormDataItemImageModel(id: images.Id, topic: images.Topic)
                    {
                        ImagesItems = images.ImagesItems.Select(
                            item => new ImageItem(
                                id: item.Id,
                            imagePath: item.ImagePath,
                            imageObservation: item.ImageObservation))
                        .ToList()
                    };

                }

                if (item is AppFormDataItemObservationModel observation)
                {
                    return new ItemFormDataObservationModel(
                        observation: observation.Observation,
                        topic: observation.Topic,
                        type: ItemFormDataEnum.Observation,
                        id: observation.Id);
                }

                return new ItemFormDataObservationModel(
                      observation: "",
                      topic: "",
                      type: ItemFormDataEnum.Empty,
                      id: "");

            }).ToList(),
            ItemName = model.ItemName,
            TemplateName = model.TemplateName,
            LawList = model.LawList.Select(item =>
            new ItemLaw(
                lawId: item.LawId,
            lawContent: item.LawTextContent))
            .ToList()
        };
    }
    public static AppItemModel ToAppItemModel(this ItemRoot model)
    {
        return new()
        {
            Id = model.Id,
            FormData = model.FormData.Select<ItemFormDataContainer, IAppFormDataItemContract>(item =>
            {
                if (item is ItemFormDataCheckboxModel checkbox)
                {
                    return new AppFormDataItemCheckboxModel(id: checkbox.Id, topic: checkbox.Topic)
                    {
                        Children = checkbox.Children.Select(item =>
                        {
                            return new AppFormDataItemCheckboxChildModel(id: item.Id, topic: item.Topic)
                            {
                                Options = item.Options.Select(option =>
                                new AppOptionModel(
                                    value: option.Value,
                                id: option.Id,
                                isChecked: option.IsChecked)).ToList(),
                            };
                        }).ToList()
                    };
                }

                if (item is ItemFormDataTextModel text)
                {
                    return new AppFormDataItemTextModel(
                        id: text.Id,
                        topic: text.Topic,
                        textData: text.TextData,
                        measurementUnit: text.MeasurementUnit);
                }

                if (item is ItemFormDataItemImageModel images)
                {
                    return new AppFormDataItemImageModel(id: images.Id, topic: images.Topic)
                    {
                        ImagesItems = images.ImagesItems.Select(
                            item => new ImagesItem(
                                id: item.Id,
                            imagePath: item.ImagePath,
                            imageObservation: item.ImageObservation))
                        .ToList()
                    };

                }

                if (item is ItemFormDataObservationModel observation)
                {
                    return new AppFormDataItemObservationModel(
                        observation: observation.Observation,
                        topic: observation.Topic,
                        type: Core.Enuns.AppFormDataType.Observação,
                        id: observation.Id);
                }

                return new AppFormDataEmptyModel();
            }).ToList(),
            ItemName = model.ItemName,
            TemplateName = model.TemplateName,
            LawList = model.LawList.Select(item =>
            new AppLawModel(
                lawId: item.LawId,
            lawTextContent: item.LawTextContent))
            .ToList()
        };
    }
}
