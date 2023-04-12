using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.DTO.FormItem;

public class ImageItem : ItemFormData
{
    public ImageItem()
    {
    }

    public ImageItem(
        string id
        , string imagePath
        , string imageObservation
    )
        : base(id: id)
    {
        Id = id;
        ImagePath = imagePath;
        ImageObservation = imageObservation;
    }

    [XmlElement(elementName: "image_path")]
    public string ImagePath
    {
        get;
        set;
    }

    [XmlElement(elementName: "image_observation")]
    public string ImageObservation
    {
        get;
        set;
    }
}