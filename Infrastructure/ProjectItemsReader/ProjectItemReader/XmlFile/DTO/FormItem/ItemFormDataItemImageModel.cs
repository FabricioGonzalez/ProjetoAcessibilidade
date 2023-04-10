using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.DTO.FormItem;

public class ItemFormDataItemImageModel : ItemFormDataContainer
{
    public ItemFormDataItemImageModel()
    {
    }

    public ItemFormDataItemImageModel(
        string id
        , string topic
        , ItemFormDataEnum type = ItemFormDataEnum.Image
    )
        : base(id: id, topic: topic, type: type)
    {
        ImagesItems = new List<ImageItem>();
    }

    [XmlArray(elementName: "images")]
    [XmlArrayItem(elementName: "image_item", Type = typeof(ImageItem))]
    public List<ImageItem> ImagesItems
    {
        get;
        set;
    }
}