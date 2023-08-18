using System.Xml.Serialization;

namespace XmlDatasource.Solution.DTO;

public class ReportItem
{
    [XmlElement(elementName: "company_info")]
    public CompanyInfoItem CompanyInfo
    {
        get;
        set;
    }

    [XmlElement(elementName: "manager_info")]
    public ManagementCompanyInfo Manager
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

    [XmlElement(elementName: "phone")]
    public int Revisao
    {
        get;
        set;
    }
}

public class ManagementCompanyInfo
{
    [XmlElement(elementName: "phone")]
    public string Telefone
    {
        get;
        set;
    } = "";

    [XmlElement(elementName: "logo_path")]
    public string LogoPath
    {
        get;
        set;
    } = "";

    [XmlElement(elementName: "web_site")]
    public string WebSite
    {
        get;
        set;
    } = "";

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

    [XmlElement(elementName: "web_site")]
    public string WebSite
    {
        get;
        set;
    } = "";
}

public sealed class CompanyInfoItem
{
    /*[XmlElement(elementName: "phone")]
    public string Telefone
    {
        get;
        set;
    } = "";*/

    [XmlElement(elementName: "report_data")]
    public DateTimeOffset Data
    {
        get;
        set;
    } = DateTime.Now;

    [XmlElement(elementName: "address")]
    public EnderecoItem Endereco
    {
        get;
        set;
    }

    [XmlElement(elementName: "logo_path")]
    public string LogoPath
    {
        get;
        set;
    } = "";

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
}