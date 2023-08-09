using System.Xml.Serialization;

namespace XmlDatasource.XmlFile.ValidationRules;

public class RuleTargetsItem
{
    [XmlText]
    public string Id
    {
        get; set;
    }
}