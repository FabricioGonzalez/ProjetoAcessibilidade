using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;

[XmlRoot(elementName: "value_trigger")]
public class ValueTriggers
{
    [XmlText]
    public string ValueTrigger
    {
        get; set;
    }
}