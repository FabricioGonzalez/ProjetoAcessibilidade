using System.Xml.Serialization;
using XmlDatasource.ProjectItems.DTO.FormItem;

namespace XmlDatasource.ProjectItems.DTO;

[XmlRoot("item")]
public class ItemRoot
{
    [XmlElement("id")]
    public string? Id
    {
        get;
        set;
    }

    [XmlElement("name")]
    public string? ItemName
    {
        get;
        set;
    }

    [XmlElement("template_name")]
    public string? TemplateName
    {
        get;
        set;
    }

    [XmlArray("table", IsNullable = true)]
    [XmlArrayItem("checkbox_items", IsNullable = true, Type = typeof(ItemFormDataCheckboxModel))]
    [XmlArrayItem("text_item", IsNullable = true, Type = typeof(ItemFormDataTextModel))]

    public List<ItemFormDataContainer> FormData
    {
        get;
        set;
    } = new();

    [XmlArray("observations", IsNullable = true)]
    [XmlArrayItem("observation", Type = typeof(ObservationModel))]
    public List<ObservationModel> Observations
    {
        get;
        set;
    } = new();

    [XmlArray("images", IsNullable = true)]
    [XmlArrayItem("image", Type = typeof(ImageItem))]
    public List<ImageItem> Images
    {
        get;
        set;
    } = new();

    [XmlArray("law")]
    [XmlArrayItem("law_item", Type = typeof(ItemLaw))]
    public List<ItemLaw> LawList
    {
        get;
        set;
    } = new();
}