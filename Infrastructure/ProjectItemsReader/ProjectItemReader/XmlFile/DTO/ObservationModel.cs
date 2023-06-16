using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.DTO;

[XmlRoot(elementName: "observation")]
public class ObservationModel
{
    public ObservationModel()
    {
    }

    public ObservationModel(
        string observation
        , string id
    )
    {
        Id = id;
        Observation = observation;
    }

    [XmlElement(elementName: "id")]
    public string Id
    {
        get;
        set;
    }
    [XmlElement(elementName: "text")]
    public string Observation
    {
        get;
        set;
    }
}