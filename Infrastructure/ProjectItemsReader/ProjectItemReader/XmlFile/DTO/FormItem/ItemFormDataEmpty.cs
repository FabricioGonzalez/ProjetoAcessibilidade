namespace ProjectItemReader.XmlFile.DTO.FormItem;
public class ItemFormDataEmpty : ItemFormDataContainer
{
    public ItemFormDataEmpty() : base()
    {

    }

    public ItemFormDataEmpty(
        string id = "",
        string topic = "",
        ItemFormDataEnum type = ItemFormDataEnum.Empty)
        : base(id, topic, type)
    {

    }
}
