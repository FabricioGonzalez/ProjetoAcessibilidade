using System.Xml.Serialization;

namespace XmlDatasource.ProjectItems.DTO.FormItem;

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
        : base(id, topic, type)
    {
        Children = new List<ItemFormDataCheckboxChildModel>();
    }

    [XmlArray("checkboxes", IsNullable = true)]
    [XmlArrayItem("checkbox_item", IsNullable = true)]
    public List<ItemFormDataCheckboxChildModel>? Children
    {
        get;
        set;
    }
}