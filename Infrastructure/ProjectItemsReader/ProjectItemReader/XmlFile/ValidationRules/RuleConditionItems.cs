using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;


[XmlRoot(elementName: "condition")]
public class RuleConditionItems
{
    [XmlElement(elementName: "operation")]
    public string Operation
    {
        get; set;
    }

    [XmlArray(elementName: "conditions_to_check")]
    public IEnumerable<ValueTriggers> Triggers
    {
        get; set;
    }

    [XmlArray(elementName: "results")]
    public IEnumerable<Results> Results
    {
        get; set;
    }
}