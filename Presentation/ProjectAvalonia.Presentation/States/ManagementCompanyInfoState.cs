using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public class ManagementCompanyInfoState : ReactiveObject
{
    private string _email = "";

    private string _logoPath = "";

    private string _nomeEmpresa = "";

    private string _responsavel = "";
    private string _telefone = "";

    private string _website = "";

    public string Telefone
    {
        get => _telefone;
        set => this.RaiseAndSetIfChanged(backingField: ref _telefone, newValue: value);
    }

    public string LogoPath
    {
        get => _logoPath;
        set => this.RaiseAndSetIfChanged(backingField: ref _logoPath, newValue: value);
    }

    public string WebSite
    {
        get => _website;
        set => this.RaiseAndSetIfChanged(backingField: ref _website, newValue: value);
    }

    public string NomeEmpresa
    {
        get => _nomeEmpresa;
        set => this.RaiseAndSetIfChanged(backingField: ref _nomeEmpresa, newValue: value);
    }

    public string Email
    {
        get => _email;
        set => this.RaiseAndSetIfChanged(backingField: ref _email, newValue: value);
    }

    public string Responsavel
    {
        get => _responsavel;
        set => this.RaiseAndSetIfChanged(backingField: ref _responsavel, newValue: value);
    }
}