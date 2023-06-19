using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;

public class RuleTargetsItem
{
    [XmlText]
    public string Id
    {
        get; set;
    }
}