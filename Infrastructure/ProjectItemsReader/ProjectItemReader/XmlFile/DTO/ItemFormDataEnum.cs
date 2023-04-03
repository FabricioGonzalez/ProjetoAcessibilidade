using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.DTO;

public enum ItemFormDataEnum
{
    [XmlEnum(Name = "text")]
    Text,

    [XmlEnum(Name = "checkbox")]
    Checkbox,

    [XmlEnum(Name = "image")]
    Image,

    [XmlEnum(Name = "observation")]
    Observation,

    Empty
}
