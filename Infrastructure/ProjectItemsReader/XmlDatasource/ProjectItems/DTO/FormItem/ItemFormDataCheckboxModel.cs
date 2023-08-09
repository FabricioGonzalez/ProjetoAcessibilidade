using System.Xml.Serialization;

namespace XmlDatasource.XmlFile.DTO.FormItem;

public class ItemFormDataCheckboxModel : ItemFormDataContainer
{
    public ItemFormDataCheckboxModel()
    {
    }

    public ItemFormDataCheckboxModel(
        string id
        , string topic
        , ItemFormDataEnum type = ItemFormDataEnum.Checkbox
    )
        : base(id: id, topic: topic, type: type)
    {
        Children = new List<ItemFormDataCheckboxChildModel>();
    }

    [XmlArray(elementName: "checkboxes")]
    [XmlArrayItem(elementName: "checkbox_item")]
    public List<ItemFormDataCheckboxChildModel> Children
    {
        get;
        set;
    }
}