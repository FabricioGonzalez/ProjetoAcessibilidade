using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;


[XmlRoot(elementName: "validation_rule")]
public class ValidationItemRoot
{
    [XmlArray(elementName: "targets")]
    public IEnumerable<RuleTargetsItem> Targets
    {
        get; set;
    }
    [XmlArray(elementName: "conditions")]
    public IEnumerable<RuleConditionItems> RuleConditions
    {
        get; set;
    }
}
