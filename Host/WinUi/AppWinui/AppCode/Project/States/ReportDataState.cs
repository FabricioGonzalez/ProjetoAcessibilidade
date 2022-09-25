using AppUsecases.Entities;

using ReactiveUI;

namespace AppWinui.AppCode.Project.States;
public class ReportDataState : ReactiveObject
{
    private string email = "";
    public string Email
    {
        get => email;
        set => this.RaiseAndSetIfChanged(ref email, value, nameof(Email));
    }

    private string endereco = "";
    public string Endereco
    {
        get => endereco;
        set => this.RaiseAndSetIfChanged(ref endereco, value, nameof(Endereco));
    }

    private string nomeEmpresa = "";
    public string NomeEmpresa
    {
        get => nomeEmpresa;
        set => this.RaiseAndSetIfChanged(ref nomeEmpresa, value, nameof(NomeEmpresa));
    }

    private string responsavel = "";
    public string Responsavel
    {
        get => responsavel;
        set => this.RaiseAndSetIfChanged(ref responsavel, value, nameof(Responsavel));
    }

    private string telefone = "";
    public string Telefone
    {
        get => telefone;
        set => this.RaiseAndSetIfChanged(ref telefone, value, nameof(Telefone));
    }
    private string uf = "";
    public string UF
    {
        get => uf;
        set => this.RaiseAndSetIfChanged(ref uf, value, nameof(UF));
    }

    private DateTime data;
    public DateTime Data
    {
        get => data;
        set => this.RaiseAndSetIfChanged(ref data, value, nameof(Data));
    }

    private string logoPath = "";
    public string LogoPath
    {
        get => logoPath;
        set => this.RaiseAndSetIfChanged(ref logoPath, value, nameof(LogoPath));
    }

    private string solutionName = "";
    public string SolutionName
    {
        get => solutionName;
        set => this.RaiseAndSetIfChanged(ref solutionName, value, nameof(SolutionName));
    }
}
