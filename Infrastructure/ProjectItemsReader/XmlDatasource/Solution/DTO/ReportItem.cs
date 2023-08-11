using System.Xml.Serialization;

namespace XmlDatasource.Solution.DTO;

public class ReportItem
{
    public CompanyInfoItem CompanyInfo
    {
        get;
        set;
    }

    [XmlArray("partners")]
    [XmlArrayItem("partner")]
    public List<PartnerItem> Partners
    {
        get;
        set;
    }


    [XmlElement(elementName: "solution_name")]
    public string SolutionName
    {
        get;
        set;
    } = "";
}

public sealed class PartnerItem
{
    [XmlElement(elementName: "partner_name")]
    public string NomeEmpresa
    {
        get;
        set;
    } = "";

    [XmlElement(elementName: "partner_logo")]
    public string PartnerLogo
    {
        get;
        set;
    } = "";
}

public sealed class CompanyInfoItem
{
    [XmlElement(elementName: "phone")]
    public string Telefone
    {
        get;
        set;
    } = "";

    [XmlElement(elementName: "report_data")]
    public DateTimeOffset Data
    {
        get;
        set;
    } = DateTime.Now;

    [XmlElement(elementName: "logo_path")]
    public string LogoPath
    {
        get;
        set;
    } = "";


    [XmlElement(elementName: "address")]
    public EnderecoItem Endereco
    {
        get;
        set;
    }

    [XmlElement(elementName: "company_name")]
    public string NomeEmpresa
    {
        get;
        set;
    } = "";

    [XmlElement(elementName: "email")]
    public string Email
    {
        get;
        set;
    } = "";

    [XmlElement(elementName: "responsable")]
    public string Responsavel
    {
        get;
        set;
    } = "";
}