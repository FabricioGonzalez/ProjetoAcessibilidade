using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;

public class RuleConditionItems
{

    [XmlElement(elementName: "operation")]
    public string Operation
    {
        get; set;
    }

    [XmlArray(elementName: "conditions_to_check")]
    [XmlArrayItem(elementName: "rules_set", type: typeof(RuleSetItem))]
    public List<RuleSetItem> RuleSetItems
    {
        get; set;
    }


}

public class RuleSetItem
{
    [XmlElement(elementName: "value_trigger")]
    public ValueTriggers ValueTrigger
    {
        get; set;
    }
    [XmlArray(elementName: "results")]
    [XmlArrayItem(elementName: "result_item", typeof(Results))]
    public List<Results> Results
    {
        get; set;
    }
}