using System.Xml.Serialization;

namespace XmlDatasource.ValidationRules.DTO;

public sealed class RuleSetItem
{
    [XmlElement(elementName: "value_trigger")]
    public ValueTrigger ValueTrigger
    {
        get;
        set;
    }

    [XmlArray(elementName: "results")]
    [XmlArrayItem(elementName: "result_item", type: typeof(Results))]
    public List<Results> Results
    {
        get;
        set;
    }
}