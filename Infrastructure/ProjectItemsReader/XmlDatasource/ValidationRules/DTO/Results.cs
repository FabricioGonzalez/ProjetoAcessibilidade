using System.Xml.Serialization;

namespace XmlDatasource.XmlFile.ValidationRules;

public class Results
{
    [XmlText]
    public string Result
    {
        get; set;
    }
}