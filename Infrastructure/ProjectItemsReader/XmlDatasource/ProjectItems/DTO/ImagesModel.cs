using System.Xml.Serialization;

namespace XmlDatasource.XmlFile.DTO;

public class ImagesModel
{

    public ImagesModel()
    {
        ImagesItems = new List<ImageItem>();
    }

    [XmlArray(elementName: "images")]
    [XmlArrayItem(elementName: "image", Type = typeof(ImageItem))]
    public List<ImageItem> ImagesItems
    {
        get;
        set;
    }
}