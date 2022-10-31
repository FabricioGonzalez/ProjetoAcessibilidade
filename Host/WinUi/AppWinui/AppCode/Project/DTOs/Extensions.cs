using AppUsecases.Project.Entities.Project;
using AppWinui.AppCode.Project.States;

namespace AppWinui.AppCode.Project.DTOs;
public static class Extensions
{
    public static ReportDataState ToReportDataState(this ReportDataModel model)
    {
        ReportDataState report = new();

        report.Data = model.Data is not null ? DateTime.Parse(model.Data) : DateTime.Now;
        report.Email = model.Email is not null ? model.Email : "";
        report.UF = model.UF is not null ? model.UF : "";
        report.LogoPath = model.LogoPath is not null ? model.LogoPath : "";
        report.Endereco = model.Endereco is not null ? model.Endereco : "";
        report.Responsavel = model.Responsavel is not null ? model.Responsavel : "";
        report.NomeEmpresa = model.NomeEmpresa is not null ? model.NomeEmpresa : "";
        report.Telefone = model.Telefone is not null ? model.Telefone : "";
        report.SolutionName = model.SolutionName is not null ? model.SolutionName : "";

        return report;
    }
}
