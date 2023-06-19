using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;
public class ValueTriggers
{
    [XmlText]
    public string ValueTrigger
    {
        get; set;
    }
}