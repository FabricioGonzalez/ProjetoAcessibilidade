using System;
using Core.Entities.App;
using Core.Entities.Solution.ReportInfo;

namespace ProjectAvalonia.Features.Project.States;

public partial class SolutionReportState
{
    [AutoNotify] private DateTimeOffset _data = DateTime.Now;

    [AutoNotify] private string _email = "";

    [AutoNotify] private string _endereco = "";

    [AutoNotify] private string _logoPath = "";

    [AutoNotify] private string _nomeEmpresa = "";

    [AutoNotify] private string _responsavel = "";

    [AutoNotify] private string _solutionName = "";

    [AutoNotify] private string _telefone = "";

    [AutoNotify] private UFModel _uF;
}

public static partial class Extensions
{
    public static SolutionReportState ToReportState(
        this SolutionInfo solutionInfo
    ) => new()
    {
        Data = solutionInfo.Data, Email = solutionInfo.Email, LogoPath = solutionInfo.LogoPath,
        NomeEmpresa = solutionInfo.NomeEmpresa, Responsavel = solutionInfo.Responsavel,
        Endereco = solutionInfo.Endereco, SolutionName = solutionInfo.SolutionName, Telefone = solutionInfo.Telefone,
        UF = solutionInfo.UF
    };

    public static SolutionInfo ToReportData(
        this SolutionReportState state
    ) =>
        new()
        {
            Data = state.Data, Email = state.Email, LogoPath = state.LogoPath, Responsavel = state.Responsavel,
            Endereco = state.Endereco, NomeEmpresa = state.NomeEmpresa, SolutionName = state.SolutionName,
            Telefone = state.Telefone, UF = state.UF
        };
}