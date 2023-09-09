using System.Xml.Serialization;

namespace Core.Entities.Solution.Project.AppItem.DataItems.Images;

public class ImagesItem
{
    public ImagesItem()
    {

    }
    public ImagesItem(
        string id
        , string imagePath
        , string imageObservation
    )
    {
        Id = id;
        ImagePath = imagePath;
        ImageObservation = imageObservation;
    }
    [XmlElement(elementName: "id")]
    public string Id
    {
        get;
        set;
    }
    [XmlElement(elementName: "path")]
    public string ImagePath
    {
        get;
        set;
    }
    [XmlElement(elementName: "observation")]
    public string ImageObservation
    {
        get;
        set;
    }
}