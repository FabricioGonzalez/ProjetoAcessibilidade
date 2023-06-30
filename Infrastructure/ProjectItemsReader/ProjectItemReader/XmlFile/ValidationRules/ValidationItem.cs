using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;

public class ValidationItem
{
    [XmlElement("target")]
    public RuleTargetsItem Target
    {
        get;
        set;
    }

    [XmlArray("conditions")]
    [XmlArrayItem("condition", typeof(RuleConditionItems))]
    public List<RuleConditionItems> RuleConditions
    {
        get;
        set;
    }
}