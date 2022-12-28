using App.Core.Entities.App;

namespace App.Core.Entities.Solution.ReportInfo;
public class SolutionInfo
{
    public string Email
    {
        get; set;
    } = "";

    public string Endereco
    {
        get; set;
    } = "";

    public string NomeEmpresa
    {
        get; set;
    } = "";
    public string Responsavel
    {
        get; set;
    } = "";
    public string Telefone
    {
        get; set;
    } = "";

    public UFModel UF
    {
        get; set;
    }

    public DateTimeOffset Data
    {
        get; set;
    } = DateTime.Now;

    public string LogoPath
    {
        get; set;
    } = "";

    public string SolutionName
    {
        get; set;
    } = "";

}
