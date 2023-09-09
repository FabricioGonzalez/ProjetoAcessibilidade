using System.Xml.Serialization;

namespace XmlDatasource.ValidationRules.DTO;

public sealed class RuleConditionItem
{
    [XmlElement(elementName: "operation")]
    public string Operation
    {
        get;
        set;
    }

    [XmlAttribute(attributeName: "rule_name")]
    public string RuleName
    {
        get;
        set;
    }

    [XmlArray(elementName: "conditions_to_check")]
    [XmlArrayItem(elementName: "rules_set", type: typeof(RuleSetItem))]
    public List<RuleSetItem> RuleSetItems
    {
        get;
        set;
    }
}