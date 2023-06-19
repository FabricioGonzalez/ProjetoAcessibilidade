using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;

public class Results
{
    [XmlText]
    public string Result
    {
        get; set;
    }
}