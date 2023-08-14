using System.Xml.Serialization;

namespace XmlDatasource.ValidationRules.DTO;

public class RuleTargetsItem
{
    [XmlText]
    public string Id
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }
}