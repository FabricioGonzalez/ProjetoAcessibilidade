using System.Xml.Serialization;

namespace XmlDatasource.XmlFile.ValidationRules;
public class ValueTriggers
{
    [XmlText]
    public string ValueTrigger
    {
        get; set;
    }
}