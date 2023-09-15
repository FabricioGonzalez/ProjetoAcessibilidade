using System.Collections.ObjectModel;
using System.Linq;

using ProjectAvalonia.Presentation.Interfaces.Services;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.ProjectItems;

using XmlDatasource.Solution.DTO;

namespace ProjectAvalonia.Features.Project.Mappings;

public static class SolutionStateMapping
{
    public static SolutionState ToSolutionState(
        this SolutionItemRoot root
        , ILocationService locationService
    ) =>
        new(locationService)
        {
            Report = root.Report.ToSolutionReportState(),
            FilePath = root.SolutionPath
            ,
            LocationItems =
                new ObservableCollection<LocationItemState>(root.ProjectItems.Select(it => it.ToLocationItem()))
        };

    public static SolutionReportState ToSolutionReportState(
        this ReportItem report
    ) =>
        new()
        {
            SolutionName = report.SolutionName,
            ManagerInfo = report.Manager.ToManagerInfoState()
            ,
            Partners =
                new ObservableCollection<PartnerLogoState>(report.Partners.Select(it => it.ToPartnerLogoState()))
            ,
            CompanyInfo = report.CompanyInfo.ToCompanyInfoState()
        };

    public static LocationItemState ToLocationItem(
        this ProjectItems item
    ) =>
        new()
        {
            Name = item.ItemName
            ,
            ItemGroup = new ObservableCollection<ItemGroupState>(item.LocationGroups.Select(it => it.ToItemGroup()))
        };

    public static ItemGroupState ToItemGroup(
        this LocationGroup item
    ) =>
        new()
        {
            Name = item.Name,
            Items = new ObservableCollection<ItemState>(item.ItemsGroup.Select(it => it.ToItem()))
        };

    public static ItemState ToItem(
        this ItemGroup item
    ) =>
        new()
        {
            Name = item.Name,
            Id = item.Id,
            ItemPath = item.ItemPath,
            TemplateName = item.TemplateName
        };

    public static CompanyInfoState ToCompanyInfoState(
        this CompanyInfoItem companyInfo
    ) =>
        new()
        {
            NomeEmpresa = companyInfo.NomeEmpresa,
            Data = companyInfo.Data.DateTime,
            Email = companyInfo.Email
            ,
            Logo = companyInfo.LogoPath,
            Endereco = companyInfo.Endereco.ToEnderecoState()
        };

    public static ManagementCompanyInfoState ToManagerInfoState(
        this ManagementCompanyInfo managerInfo
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


    public static EnderecoState ToEnderecoState(
        this EnderecoItem endereco
    ) =>
        new()
        {
            Bairro = endereco.Bairro,
            Cidade = endereco.Cidade,
            Cep = endereco.Cep,
            Uf = endereco.UF,
            Logradouro = endereco.Logradouro,
            Numero = endereco.Numero
        };

    public static PartnerLogoState ToPartnerLogoState(
        this PartnerItem partner
    ) =>
        new()
        {
            Logo = partner.PartnerLogo,
            Website = partner.WebSite,
            Name = partner.NomeEmpresa
        };
}