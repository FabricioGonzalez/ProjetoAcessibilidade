using System.Xml.Serialization;
using XmlDatasource.ProjectItems.DTO.FormItem;

namespace XmlDatasource.ProjectItems.DTO;

[XmlRoot("item")]
public class ItemRoot
{
    [XmlElement("id")]
    public string Id
    {
        get;
        set;
    }

    [XmlElement("name")]
    public string ItemName
    {
        get;
        set;
    }

    [XmlElement("template_name")]
    public string TemplateName
    {
        get;
        set;
    }

    [XmlArray("table", IsNullable = true)]
    [XmlArrayItem("checkbox_items", IsNullable = true, Type = typeof(ItemFormDataCheckboxModel))]
    [XmlArrayItem("text_item", IsNullable = true, Type = typeof(ItemFormDataTextModel))]

    public List<ItemFormDataContainer>? FormData
    {
        get;
        set;
    }

    [XmlArray("observations", IsNullable = true)]
    [XmlArrayItem("observation", Type = typeof(ObservationModel))]
    public List<ObservationModel> Observations
    {
        get;
        set;
    }

    [XmlArray("images", IsNullable = true)]
    [XmlArrayItem("image", Type = typeof(ImageItem))]
    public List<ImageItem> Images
    {
        get;
        set;
    }

    [XmlArray("law")]
    [XmlArrayItem("law_item", Type = typeof(ItemLaw))]
    public List<ItemLaw> LawList
    {
        get;
        set;
    }
}

/*public static class Extensions
{
    public static ItemRoot ToItemRoot(
        this AppItemModel model
    ) =>
        new()
        {
            Id = model.Id, FormData = model.FormData.Select<IAppFormDataItemContract, ItemFormDataContainer>(
                item =>
                {
                    if (item is AppFormDataItemCheckboxModel checkbox)
                    {
                        return new ItemFormDataCheckboxModel(checkbox.Id, checkbox.Topic)
                        {
                            Children = checkbox.Children.Select(childCheckbox =>
                            {
                                return new ItemFormDataCheckboxChildModel(childCheckbox.Id, childCheckbox.Topic
                                    , childCheckbox.IsValid)
                                {
                                    Options = childCheckbox.Options.Select(option =>
                                            new ItemOptionModel(
                                                option.Value,
                                                option.Id,
                                                option.IsChecked))
                                        .ToList()
                                    , TextItems = childCheckbox.TextItems.Select(textItem =>
                                            new ItemFormDataTextModel(textItem.Id, textItem.Topic
                                                , textData: textItem.TextData
                                                , measurementUnit: textItem.MeasurementUnit))
                                        .ToList()
                                };
                            }).ToList()
                        };
                    }

                    if (item is AppFormDataItemTextModel text)
                    {
                        return new ItemFormDataTextModel(
                            text.Id,
                            text.Topic,
                            textData: text.TextData,
                            measurementUnit: text.MeasurementUnit);
                    }

                    return new ItemFormDataTextModel(
                        "",
                        "",
                        textData: "",
                        type: ItemFormDataEnum.Empty);
                }).ToList()
            , Observations = model.Observations
                .Select(x => new ObservationModel { Id = x.Id, Observation = x.ObservationText }).ToList()
            , Images =
                model.Images
                    .Where(x => !string.IsNullOrWhiteSpace(x.ImagePath))
                    .Select(x => new ImagesItem(x.Id, x.ImagePath, x.ImageObservation))
                    .ToList()
            , ItemName = model.ItemName, TemplateName = model.TemplateName, LawList = model.LawList.Select(
                    item =>
                        new ItemLaw(
                            item.LawId,
                            item.LawTextContent))
                .ToList()
        };

    public static AppItemModel ToAppItemModel(
        this ItemRoot model
    ) =>
        new()
        {
            Id = model.Id, FormData = model.FormData.Select<ItemFormDataContainer, IAppFormDataItemContract>(
                item =>
                {
                    if (item is ItemFormDataCheckboxModel checkbox)
                    {
                        return new AppFormDataItemCheckboxModel(checkbox.Id, checkbox.Topic)
                        {
                            Children = checkbox.Children.Select(childCheckbox =>
                            {
                                return new AppFormDataItemCheckboxChildModel(childCheckbox.Id, childCheckbox.Topic
                                    , childCheckbox.IsInvalid ?? false)
                                {
                                    Options = childCheckbox.Options.Select(option =>
                                        new AppOptionModel(
                                            option.Value,
                                            option.Id,
                                            option.IsChecked)).ToList()
                                    , TextItems = childCheckbox.TextItems.Select(textItem =>
                                            new AppFormDataItemTextModel(textItem.Id, textItem.Topic
                                                , textData: textItem.TextData
                                                , measurementUnit: textItem.MeasurementUnit))
                                        .ToList()
                                };
                            }).ToList()
                        };
                    }

                    if (item is ItemFormDataTextModel text)
                    {
                        return new AppFormDataItemTextModel(
                            text.Id,
                            text.Topic,
                            textData: text.TextData,
                            measurementUnit: text.MeasurementUnit);
                    }

                    return new AppFormDataEmptyModel();
                }).ToList()
            , Observations = model.Observations.Select(x => new Core.Entities.Solution.Project.AppItem.ObservationModel
                { Id = x.Id, ObservationText = x.Observation })
            , Images = model
                .Images
                .Where(x => !string.IsNullOrWhiteSpace(x.ImagePath))
                .Select(x => new ImagesItem(x.Id, x.ImagePath, x.ImageObservation))
            , ItemName = model.ItemName, TemplateName = model.TemplateName, LawList = model.LawList.Select(
                    item =>
                        new AppLawModel(
                            item.LawId,
                            item.LawTextContent))
                .ToList()
        };
}*/