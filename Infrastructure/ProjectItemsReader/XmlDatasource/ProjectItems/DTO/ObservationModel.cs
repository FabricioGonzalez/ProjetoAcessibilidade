using System.Xml.Serialization;

namespace XmlDatasource.ProjectItems.DTO;

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