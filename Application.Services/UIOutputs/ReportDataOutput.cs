using System.Text.Json.Serialization;

namespace SystemApplication.Services.UIOutputs;
public class ReportDataOutput : NotifierBaseClass
{
    private string? email;
    [JsonPropertyName("email")]
    public string Email
    {
        get => email;
        set => SetAtributeValue(ref email, value, nameof(Email));
    }

    private string? endereco;
    [JsonPropertyName("endereco")]
    public string Endereco
    {
        get => endereco;
        set => SetAtributeValue(ref endereco, value);
    }

    private string? nomeEmpresa;
    [JsonPropertyName("nome_empresa")]
    public string NomeEmpresa
    {
        get => nomeEmpresa;
        set => SetAtributeValue(ref nomeEmpresa, value);
    }

    private string? responsavel;
    [JsonPropertyName("responsavel")]
    public string Responsavel
    {
        get => responsavel;
        set => SetAtributeValue(ref responsavel, value);
    }

    private string? telefone;
    [JsonPropertyName("telefone")]
    public string Telefone
    {
        get => telefone;
        set => SetAtributeValue(ref telefone, value);
    }

    private string? uf;
    [JsonPropertyName("uf")]
    public string UF
    {
        get => uf;
        set => SetAtributeValue(ref uf, value);
    }

    private string? data;
    [JsonPropertyName("data")]
    public string Data
    {
        get => data;
        set => SetAtributeValue(ref data, value);
    }
    private string? logo;
    [JsonPropertyName("logo")]
    public string LogoPath
    {
        get => logo;
        set => SetAtributeValue(ref logo, value);
    }

    private string? solutionName;
    [JsonPropertyName("solutionName")]
    public string SolutionName
    {
        get => solutionName;
        set => SetAtributeValue(ref solutionName, value);
    }
}
