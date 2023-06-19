using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;
public class ValidationItem
{
    [XmlArray(elementName: "targets")]
    [XmlArrayItem(elementName: "target", type: typeof(RuleTargetsItem))]
    public List<RuleTargetsItem> Targets
    {
        get; set;
    }
    [XmlArray(elementName: "conditions")]
    [XmlArrayItem(elementName: "condition", type: typeof(RuleConditionItems))]
    public List<RuleConditionItems> RuleConditions
    {
        get; set;
    }
}
