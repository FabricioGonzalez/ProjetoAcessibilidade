using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SystemApplication.Services.UIOutputs;
public class ReportDataOutput : NotifierBaseClass
{
    private string? email;
    [JsonPropertyName("email")]
    public string Email
    {
        get => email; set
        {
            SetAtributeValue(ref email, value);
            NotifyPropertyChanged(nameof(Email));
        }
    }

    private string? endereco;
    [JsonPropertyName("endereco")]
    public string Endereco
    {
        get => endereco; set
        {
            SetAtributeValue(ref endereco, value);
            NotifyPropertyChanged(nameof(Endereco));
        }
    }

    private string? nomeEmpresa;
    [JsonPropertyName("nome_empresa")]
    public string NomeEmpresa
    {
        get => nomeEmpresa; set
        {
            SetAtributeValue(ref nomeEmpresa, value);
            NotifyPropertyChanged(nameof(NomeEmpresa));
        }
    }

    private string? responsavel;
    [JsonPropertyName("responsavel")]
    public string Responsavel
    {
        get => responsavel; set
        {
            SetAtributeValue(ref responsavel, value);
            NotifyPropertyChanged(nameof(Responsavel));
        }
    }

    private string? telefone;
   [JsonPropertyName("telefone")]
    public string Telefone
    {
        get => telefone; set
        {
            SetAtributeValue(ref telefone, value);
            NotifyPropertyChanged(nameof(Telefone));
        }
    }

    private string? uf;
    [JsonPropertyName("uf")]
    public string UF
    {
        get => uf; set
        {
            SetAtributeValue(ref uf, value);
            NotifyPropertyChanged(nameof(UF));
        }
    }

    private string? data;
    [JsonPropertyName("data")]
    public string Data
    {
        get => data; set
        {
            SetAtributeValue(ref data, value);
            NotifyPropertyChanged(nameof(Data));
        }
    }
    private string? logo;
    [JsonPropertyName("logo")]
    public string LogoPath
    {
        get => logo; set
        {
            SetAtributeValue(ref logo, value);
            NotifyPropertyChanged(nameof(LogoPath));
        }
    }
    
    private string? solutionName;
    [JsonPropertyName("solutionName")]
    public string SolutionName
    {
        get => solutionName; set
        {
            SetAtributeValue(ref solutionName, value);
            NotifyPropertyChanged(nameof(SolutionName));
        }
    }
}
