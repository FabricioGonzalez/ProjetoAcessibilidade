namespace AppRepositories.Solution.Dto;

public sealed class ReportItem
{
    public string Email
    {
        get;
        set;
    } = "";


    public EnderecoItem Endereco
    {
        get;
        set;
    }


    public string NomeEmpresa
    {
        get;
        set;
    } = "";


    public string Responsavel
    {
        get;
        set;
    } = "";


    public string Telefone
    {
        get;
        set;
    } = "";


    public DateTimeOffset Data
    {
        get;
        set;
    } = DateTime.Now;


    public string LogoPath
    {
        get;
        set;
    } = "";


    public string SolutionName
    {
        get;
        set;
    } = "";
}