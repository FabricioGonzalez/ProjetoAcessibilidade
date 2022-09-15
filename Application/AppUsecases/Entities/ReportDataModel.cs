using System.Text.Json.Serialization;

namespace AppUsecases.Entities;
public class ReportDataModel
{
    [JsonPropertyName("email")]
    public string Email
    {
        get;set;
    }

    [JsonPropertyName("endereco")]
    public string Endereco
    {
        get; set;
    }

    [JsonPropertyName("nome_empresa")]
    public string NomeEmpresa
    {
        get; set;
    }

    
    [JsonPropertyName("responsavel")]
    public string Responsavel
    {
        get; set;
    }

    [JsonPropertyName("telefone")]
    public string Telefone
    {
        get; set;
    }

    [JsonPropertyName("uf")]
    public string UF
    {
        get; set;
    }

    [JsonPropertyName("data")]
    public string Data
    {
        get; set;
    }
    [JsonPropertyName("logo")]
    public string LogoPath
    {
        get; set;
    }

    [JsonPropertyName("solutionName")]
    public string SolutionName
    {
        get; set;
    }
}
