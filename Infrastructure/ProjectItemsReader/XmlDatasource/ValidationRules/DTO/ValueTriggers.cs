using System.Xml.Serialization;

namespace XmlDatasource.ValidationRules.DTO;

public class ValueTriggers
{
    [XmlText]
    public string ValueTrigger
    {
        get;
        set;
    }
}