using System.Xml.Serialization;

namespace XmlDatasource.InternalAppFiles.DTO;

public class ReportItem
{
    [XmlElement(elementName: "email")]
    public string Email
    {
        get;
        set;
    } = "";

    [XmlElement(elementName: "address")]
    public string Endereco
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

    [XmlElement(elementName: "responsable")]
    public string Responsavel
    {
        get;
        set;
    } = "";

    [XmlElement(elementName: "phone")]
    public string Telefone
    {
        get;
        set;
    } = "";

    [XmlElement(elementName: "report_uf")]
    public string UF
    {
        get;
        set;
    }

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

    [XmlElement(elementName: "solution_name")]
    public string SolutionName
    {
        get;
        set;
    } = "";
}