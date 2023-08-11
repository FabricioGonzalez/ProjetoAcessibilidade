using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public sealed class EnderecoState : ReactiveObject
{
    private string _bairro = "";
    private string _cep = "";

    private string _cidade = "";

    private string _logradouro = "";
    private string _numero;
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

    public string Numero
    {
        get => _numero;
        set => this.RaiseAndSetIfChanged(backingField: ref _numero, newValue: value);
    }

    public string Cep
    {
        get => _cep;
        set => this.RaiseAndSetIfChanged(backingField: ref _cep, newValue: value);
    }

    public string Bairro
    {
        get => _bairro;
        set => this.RaiseAndSetIfChanged(backingField: ref _bairro, newValue: value);
    }
}