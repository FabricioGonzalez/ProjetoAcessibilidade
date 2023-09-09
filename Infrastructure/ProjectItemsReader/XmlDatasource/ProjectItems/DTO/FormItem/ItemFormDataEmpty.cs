namespace XmlDatasource.ProjectItems.DTO.FormItem;

public class ItemFormDataEmpty : ItemFormDataContainer
{
    public ItemFormDataEmpty()
    {
    }

    public ItemFormDataEmpty(
        string id = ""
        , string topic = ""
        , ItemFormDataEnum type = ItemFormDataEnum.Empty
    )
        : base(id: id, topic: topic, type: type)
    {
    }
}