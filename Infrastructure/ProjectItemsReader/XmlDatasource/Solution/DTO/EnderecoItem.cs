using System.Xml.Serialization;

namespace XmlDatasource.Solution.DTO;

public class EnderecoItem
{
    [XmlElement("logradouro")]
    public string Logradouro
    {
        get;
        set;
    }

    [XmlElement("numero")]
    public string Numero
    {
        get;
        set;
    }

    [XmlElement("bairro")]
    public string Bairro
    {
        get;
        set;
    }   
    [XmlElement("cep")]
    public string Cep
    {
        get;
        set;
    }

    [XmlElement("cidade")]
    public string Cidade
    {
        get;
        set;
    }

    [XmlElement("uf")]
    public string UF
    {
        get;
        set;
    }
}