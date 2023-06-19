using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;

[XmlRoot(elementName: "target")]
public class RuleTargetsItem
{
    [XmlText]
    public string Id
    {
        get; set;
    }
}