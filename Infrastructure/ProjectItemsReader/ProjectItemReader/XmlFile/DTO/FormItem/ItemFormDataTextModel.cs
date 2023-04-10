using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.DTO.FormItem;

public class ItemFormDataTextModel : ItemFormDataContainer
{
    public ItemFormDataTextModel()
    {
    }

    public ItemFormDataTextModel(
        string id
        , string topic
        , ItemFormDataEnum type = ItemFormDataEnum.Text
        , string textData = ""
        , string? measurementUnit = null
    )
        : base(id: id, topic: topic, type: type)
    {
        TextData = textData;
        MeasurementUnit = measurementUnit;
    }

    [XmlElement(elementName: "value")]
    public string TextData
    {
        get;
        set;
    } = "";

    [XmlElement(elementName: "measurement_unit", IsNullable = true)]
    public string? MeasurementUnit
    {
        get;
        set;
    }
}