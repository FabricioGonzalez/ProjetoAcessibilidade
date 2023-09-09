using System.Xml.Serialization;

namespace XmlDatasource.ValidationRules.DTO;

public class Results
{
    [XmlText]
    public string Result
    {
        get;
        set;
    }
}