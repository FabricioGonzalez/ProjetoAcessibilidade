using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.DTO;

public class ImageItem
{
    public ImageItem()
    {
    }

    public ImageItem(
        string id
        , string imagePath
        , string imageObservation
    )
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
    [XmlElement(elementName: "id")]
    public string Id
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