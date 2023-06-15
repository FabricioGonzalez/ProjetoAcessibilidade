using System.Xml.Serialization;

using ProjectItemReader.XmlFile.DTO.FormItem;

using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Images;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Text;
using ProjetoAcessibilidade.Core.Enuns;

namespace ProjectItemReader.XmlFile.DTO;

[XmlRoot(elementName: "item")]
public class ItemRoot
{
    [XmlElement(elementName: "id")]
    public string Id
    {
        get;
        set;
    }

    [XmlElement(elementName: "name")]
    public string ItemName
    {
        get;
        set;
    }

    [XmlElement(elementName: "template_name")]
    public string TemplateName
    {
        get;
        set;
    }

    [XmlArray(elementName: "table")]
    [XmlArrayItem(elementName: "checkbox_items", Type = typeof(ItemFormDataCheckboxModel))]
    [XmlArrayItem(elementName: "text_item", Type = typeof(ItemFormDataTextModel))]
    [XmlArrayItem(elementName: "image_items", Type = typeof(ItemFormDataItemImageModel))]
    [XmlArrayItem(elementName: "observation_item", Type = typeof(ItemFormDataObservationModel))]

    public List<ItemFormDataContainer> FormData
    {
        get;
        set;
    }

    [XmlArray(elementName: "law")]
    [XmlArrayItem(elementName: "law_item", Type = typeof(ItemLaw))]
    public List<ItemLaw> LawList
    {
        get;
        set;
    }
}

public static class Extensions
{
    public static ItemRoot ToItemRoot(
        this AppItemModel model
    ) =>
        new()
        {
            Id = model.Id,
            FormData = model.FormData.Select<IAppFormDataItemContract, ItemFormDataContainer>(
                selector: item =>
                {
                    if (item is AppFormDataItemCheckboxModel checkbox)
                    {
                        return new ItemFormDataCheckboxModel(id: checkbox.Id, topic: checkbox.Topic)
                        {
                            Children = checkbox.Children.Select(selector: item =>
                            {
                                return new ItemFormDataCheckboxChildModel(id: item.Id, topic: item.Topic)
                                {
                                    Options = item.Options.Select(selector: option =>
                                        new ItemOptionModel(
                                            value: option.Value,
                                            id: option.Id,
                                            isChecked: option.IsChecked))
                                    .ToList(),
                                    TextItems = item.TextItems.Select(textItem =>
                                   new ItemFormDataTextModel(id: textItem.Id, topic: textItem.Topic, textData: textItem.TextData, measurementUnit: textItem.MeasurementUnit))
                                    .ToList()
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
                            ImagesItems = images?
                            .ImagesItems?
                            .Select(
                                    selector: item => new ImageItem(
                                        id: item.Id,
                                        imagePath: item.ImagePath,
                                        imageObservation: item.ImageObservation))
                                .ToList() ?? Enumerable.Empty<ImageItem>().ToList()
                        };
                    };


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
            LawList = model.LawList.Select(
                    selector: item =>
                        new ItemLaw(
                            lawId: item.LawId,
                            lawContent: item.LawTextContent))
                .ToList()
        };

    public static AppItemModel ToAppItemModel(
        this ItemRoot model
    ) =>
        new()
        {
            Id = model.Id,
            FormData = model.FormData.Select<ItemFormDataContainer, IAppFormDataItemContract>(
                selector: item =>
                {
                    if (item is ItemFormDataCheckboxModel checkbox)
                    {
                        return new AppFormDataItemCheckboxModel(id: checkbox.Id, topic: checkbox.Topic)
                        {
                            Children = checkbox.Children.Select(selector: item =>
                            {
                                return new AppFormDataItemCheckboxChildModel(id: item.Id, topic: item.Topic)
                                {
                                    Options = item.Options.Select(selector: option =>
                                        new AppOptionModel(
                                            value: option.Value,
                                            id: option.Id,
                                            isChecked: option.IsChecked)).ToList(),
                                    TextItems = item.TextItems.Select(textItem =>
                                    new AppFormDataItemTextModel(id: textItem.Id, topic: textItem.Topic, textData: textItem.TextData, measurementUnit: textItem.MeasurementUnit))
                                    .ToList()
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
                                    selector: item => new ImagesItem(
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
                            type: AppFormDataType.Observação,
                            id: observation.Id);
                    }

                    return new AppFormDataEmptyModel();
                }).ToList(),
            ItemName = model.ItemName,
            TemplateName = model.TemplateName,
            LawList = model.LawList.Select(
                    selector: item =>
                        new AppLawModel(
                            lawId: item.LawId,
                            lawTextContent: item.LawTextContent))
                .ToList()
        };
}