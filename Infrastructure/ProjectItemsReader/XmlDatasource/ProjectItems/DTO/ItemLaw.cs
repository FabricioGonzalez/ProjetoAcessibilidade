using System.Xml.Serialization;

namespace XmlDatasource.ProjectItems.DTO;

public class ItemLaw
{
    public ItemLaw()
    {
    }

    public ItemLaw(
        string lawId
        , string lawContent
    )
    {
        LawId = lawId;
        LawTextContent = lawContent;
    }

    [XmlElement(elementName: "id")]
    public string LawId
    {
        get;
        set;
    }

    [XmlElement(elementName: "content")]
    public string LawTextContent
    {
        get;
        set;
    }
}