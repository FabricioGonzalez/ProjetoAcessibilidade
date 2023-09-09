using System.Xml.Serialization;

namespace XmlDatasource.ValidationRules.DTO;

public sealed class ValueTrigger
{
    [XmlElement(elementName: "checking_operation")]
    public string Operation
    {
        get;
        set;
    }

    [XmlElement(elementName: "target_value")]
    public string TargetValue
    {
        get;
        set;
    }

    [XmlElement(elementName: "target_id")]
    public string TargetId
    {
        get;
        set;
    }
}