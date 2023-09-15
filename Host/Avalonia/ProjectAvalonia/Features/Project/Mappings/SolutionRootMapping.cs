using System.Collections.Generic;
using System.Linq;

using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.ProjectItems;

using XmlDatasource.Solution.DTO;

namespace ProjectAvalonia.Features.Project.Mappings;

public static class SolutionRootMapping
{
    public static SolutionItemRoot ToSolutionItemRoot(
        this SolutionState state
    ) =>
        new()
        {
            Report = state.Report.ToSolutionReportItem()
            ,
            ProjectItems =
                new List<ProjectItems>(state.LocationItems.Select(it => it.ToProjectItem()))
            ,
            SolutionPath = state.FilePath
        };

    public static ReportItem ToSolutionReportItem(
        this SolutionReportState report
    ) =>
        new()
        {
            SolutionName = report.SolutionName,
            Manager = report.ManagerInfo.ToManagerInfoItem()
            ,
            Partners =
                new List<PartnerItem>(report.Partners.Select(it => it.ToPartnerLogoItem()))
            ,
            CompanyInfo = report.CompanyInfo.ToCompanyInfoItem()
        };

    public static ProjectItems ToProjectItem(
        this LocationItemState item
    ) =>
        new()
        {
            ItemName = item.Name
            ,
            LocationGroups = new List<LocationGroup>(item.ItemGroup.Select(it => it.ToLocationGroup()))
        };

    public static LocationGroup ToLocationGroup(
        this ItemGroupState item
    ) =>
        new()
        {
            Name = item.Name,
            ItemsGroup = new List<ItemGroup>(item.Items.Select(it => it.ToItemGroup()))
        };

    public static ItemGroup ToItemGroup(
        this ItemState item
    ) =>
        new()
        {
            Name = item.Name,
            Id = item.Id,
            ItemPath = item.ItemPath,
            TemplateName = item.TemplateName
        };

    public static CompanyInfoItem ToCompanyInfoItem(
        this CompanyInfoState companyInfo
    ) =>
        new()
        {
            NomeEmpresa = companyInfo.NomeEmpresa,
            Data = companyInfo.Data.Value,
            Email = companyInfo.Email,
            LogoPath = companyInfo.Logo,
            Endereco = companyInfo.Endereco.ToEnderecoItem()
        };

    public static ManagementCompanyInfo ToManagerInfoItem(
        this ManagementCompanyInfoState managerInfo
    ) =>
        new()
        {
            NomeEmpresa = managerInfo.NomeEmpresa,
            LogoPath = managerInfo.LogoPath
            ,
            Responsavel = managerInfo.Responsavel,
            Telefone = managerInfo.Telefone,
            Email = managerInfo.Email
            ,
            WebSite = managerInfo.WebSite
        };


    public static EnderecoItem ToEnderecoItem(
        this EnderecoState endereco
    ) =>
        new()
        {
            Bairro = endereco.Bairro,
            Cidade = endereco.Cidade,
            UF = endereco.Uf,
            Cep = endereco.Cep,
            Logradouro = endereco.Logradouro,
            Numero = endereco.Numero
        };

    public static PartnerItem ToPartnerLogoItem(
        this PartnerLogoState partner
    ) =>
        new()
        {
            PartnerLogo = partner.Logo,
            NomeEmpresa = partner.Name,
            WebSite = partner.Website
        };
}
