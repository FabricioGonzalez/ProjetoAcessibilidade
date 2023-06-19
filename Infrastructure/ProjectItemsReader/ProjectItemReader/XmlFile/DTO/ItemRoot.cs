using System.Xml.Serialization;

using Core.Entities.Solution.Project.AppItem;
using Core.Entities.Solution.Project.AppItem.DataItems.Images;

using ProjectItemReader.XmlFile.DTO.FormItem;

using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Text;

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


    public List<ItemFormDataContainer> FormData
    {
        get;
        set;
    }

    [XmlArray(elementName: "observations")]
    [XmlArrayItem(elementName: "observation", Type = typeof(ObservationModel))]
    public List<ObservationModel> Observations
    {
        get;
        set;
    }
    [XmlArray(elementName: "images")]
    [XmlArrayItem(elementName: "image", Type = typeof(ImagesItem))]
    public List<ImagesItem> Images
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

                    return new ItemFormDataTextModel(
                          id: "",
                            topic: "",
                            textData: "",
                            type: ItemFormDataEnum.Empty);
                }).ToList(),
            Observations = model.Observations.Select(x => new ObservationModel() { Id = x.Id, Observation = x.ObservationText }).ToList(),
            Images = model.Images.Select(x => new ImagesItem(x.Id, x.ImagePath, x.ImageObservation)).ToList(),
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

                    return new AppFormDataEmptyModel();
                }).ToList(),
            Observations = model.Observations.Select(x => new Core.Entities.Solution.Project.AppItem.ObservationModel() { Id = x.Id, ObservationText = x.Observation }),
            Images = model.Images.Select(x => new ImagesItem(x.Id, x.ImagePath, x.ImageObservation)),
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