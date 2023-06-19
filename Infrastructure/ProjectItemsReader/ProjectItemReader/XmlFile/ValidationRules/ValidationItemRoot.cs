using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;


[XmlRoot(elementName: "validation_rules")]
public class ValidationItemRoot
{
    [XmlArray]
    [XmlArrayItem(elementName: "rule")]
    public List<ValidationItem> Rules
    {
        get; set;
    }
}
