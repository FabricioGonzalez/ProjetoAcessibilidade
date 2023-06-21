using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;


[XmlRoot(elementName: "validation_rules")]
public class ValidationItemRoot
{
    [XmlElement(elementName: "rule")]
    public List<ValidationItem> Rules
    {
        get; set;
    }
}
