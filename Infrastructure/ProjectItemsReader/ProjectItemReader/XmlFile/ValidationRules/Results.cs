using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.ValidationRules;

[XmlRoot(elementName: "result_item")]
public class Results
{
    [XmlText]
    public string Result
    {
        get; set;
    }
}