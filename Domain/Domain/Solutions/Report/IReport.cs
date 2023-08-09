using Domain.Solutions.ValueObjects;

namespace Domain.Solutions.Report;

public interface IReport
{
    public string Email
    {
        get;
        set;
    }

    public Endereco Endereco
    {
        get;
        set;
    }


    public string NomeEmpresa
    {
        get;
        set;
    }

    public string Responsavel
    {
        get;
        set;
    }

    public string Telefone
    {
        get;
        set;
    }

    public DateTimeOffset Data
    {
        get;
        set;
    }

    public string LogoPath
    {
        get;
        set;
    }

    public string SolutionName
    {
        get;
        set;
    }
}