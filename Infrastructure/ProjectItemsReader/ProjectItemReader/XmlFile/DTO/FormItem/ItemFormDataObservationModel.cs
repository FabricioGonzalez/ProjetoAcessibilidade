using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.DTO.FormItem;

public class ItemFormDataObservationModel : ItemFormDataContainer
{
    public ItemFormDataObservationModel()
    {
    }

    public ItemFormDataObservationModel(
        string observation
        , string id
        , string topic
        , ItemFormDataEnum type = ItemFormDataEnum.Observation
    )
        : base(id: id, topic: topic, type: type)
    {
        Observation = observation;
    }

    [XmlElement(elementName: "observation")]
    public string Observation
    {
        get;
        set;
    }
}