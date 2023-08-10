using AppRepositories.Solution.Dto;

namespace XmlDatasource.Solution.Mappers;

public static class ReportItemExtensions
{
    public static ReportItem ToReportItem(
        this DTO.ReportItem reportItem
    ) => new()
    {
        Data = reportItem.Data, Email = reportItem.Email, SolutionName = reportItem.SolutionName
        , Endereco = reportItem.Endereco.ToEnderecoItem(), Responsavel = reportItem.Responsavel
        , Telefone = reportItem.Telefone, LogoPath = reportItem.LogoPath, NomeEmpresa = reportItem.NomeEmpresa
    };

    public static DTO.ReportItem ToReportItemDto(
        this ReportItem reportItem
    ) => new()
    {
        Data = reportItem.Data, Email = reportItem.Email, SolutionName = reportItem.SolutionName
        , Endereco = reportItem.Endereco.ToEnderecoItemDto(), Responsavel = reportItem.Responsavel
        , Telefone = reportItem.Telefone, LogoPath = reportItem.LogoPath, NomeEmpresa = reportItem.NomeEmpresa
    };
}