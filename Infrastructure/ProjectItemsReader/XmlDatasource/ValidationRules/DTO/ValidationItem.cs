using System.Xml.Serialization;

namespace XmlDatasource.ValidationRules.DTO;

public class ValidationItem
{
    [XmlElement("target")]
    public RuleTargetsItem Target
    {
        get;
        set;
    }

    [XmlArray("conditions")]
    [XmlArrayItem(elementName: "condition", type: typeof(RuleConditionItem))]
    public List<RuleConditionItem> RuleConditions
    {
        get;
        set;
    }
}