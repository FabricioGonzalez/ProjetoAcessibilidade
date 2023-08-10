using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public class SolutionReportState : ReactiveObject
{
    private DateTimeOffset _data = DateTimeOffset.Now;

    private string _email = "";

    private EnderedoState _endereco = new();

    private string _logoPath = "";

    private string _nomeEmpresa = "";

    private string _responsavel = "";
    private string _solutionName = "";

    private string _telefone = "";

    public string Telefone
    {
        get => _telefone;
        set => this.RaiseAndSetIfChanged(backingField: ref _telefone, newValue: value);
    }

    public string SolutionName
    {
        get => _solutionName;
        set => this.RaiseAndSetIfChanged(backingField: ref _solutionName, newValue: value);
    }

    public string LogoPath
    {
        get => _logoPath;
        set => this.RaiseAndSetIfChanged(backingField: ref _logoPath, newValue: value);
    }

    public string NomeEmpresa
    {
        get => _nomeEmpresa;
        set => this.RaiseAndSetIfChanged(backingField: ref _nomeEmpresa, newValue: value);
    }

    public string Responsavel
    {
        get => _responsavel;
        set => this.RaiseAndSetIfChanged(backingField: ref _responsavel, newValue: value);
    }

    public DateTimeOffset Data
    {
        get => _data;
        set => this.RaiseAndSetIfChanged(backingField: ref _data, newValue: value);
    }

    public string Email
    {
        get => _email;
        set => this.RaiseAndSetIfChanged(backingField: ref _email, newValue: value);
    }

    public EnderedoState Endereco
    {
        get => _endereco;
        set => this.RaiseAndSetIfChanged(backingField: ref _endereco, newValue: value);
    }
}

public class EnderedoState : ReactiveObject
{
    private string _bairro = "";

    private string _cidade = "";

    private string _logradouro = "";
    private int _numero;
    private string _uf = "";

    public string Uf
    {
        get => _uf;
        set => this.RaiseAndSetIfChanged(backingField: ref _uf, newValue: value);
    }

    public string Cidade
    {
        get => _cidade;
        set => this.RaiseAndSetIfChanged(backingField: ref _cidade, newValue: value);
    }

    public string Logradouro
    {
        get => _logradouro;
        set => this.RaiseAndSetIfChanged(backingField: ref _logradouro, newValue: value);
    }

    public int Numero
    {
        get => _numero;
        set => this.RaiseAndSetIfChanged(backingField: ref _numero, newValue: value);
    }

    public string Bairro
    {
        get => _bairro;
        set => this.RaiseAndSetIfChanged(backingField: ref _bairro, newValue: value);
    }
}