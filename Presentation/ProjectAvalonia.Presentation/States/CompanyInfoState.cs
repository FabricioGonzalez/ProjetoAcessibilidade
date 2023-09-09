using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public sealed class CompanyInfoState : ReactiveObject
{
    private DateTime? _data = DateTime.Now;

    private string _email = "";

    private EnderecoState _endereco = new();

    private string _logo = "";

    private string _nomeEmpresa = "";
    private string _responsavel = "";

    private string _telefone = "";

    public string NomeEmpresa
    {
        get => _nomeEmpresa;
        set => this.RaiseAndSetIfChanged(backingField: ref _nomeEmpresa, newValue: value);
    }

    public string Logo
    {
        get => _logo;
        set => this.RaiseAndSetIfChanged(backingField: ref _logo, newValue: value);
    }

    public string Telefone
    {
        get => _telefone;
        set => this.RaiseAndSetIfChanged(backingField: ref _telefone, newValue: value);
    }

    public string Responsavel
    {
        get => _responsavel;
        set => this.RaiseAndSetIfChanged(backingField: ref _responsavel, newValue: value);
    }

    public DateTime? Data
    {
        get => _data;
        set => this.RaiseAndSetIfChanged(backingField: ref _data, newValue: value);
    }

    public string Email
    {
        get => _email;
        set => this.RaiseAndSetIfChanged(backingField: ref _email, newValue: value);
    }


    public EnderecoState Endereco
    {
        get => _endereco;
        set => this.RaiseAndSetIfChanged(backingField: ref _endereco, newValue: value);
    }
}