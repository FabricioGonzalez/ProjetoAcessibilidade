using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Common;

using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Presentation.Interfaces.Services;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.ProjectItems;

using XmlDatasource.Solution;
using XmlDatasource.Solution.DTO;

namespace ProjectAvalonia.Features.Project.Services;

public class SolutionService
{
    private readonly ILocationService _locationService;
    private readonly SolutionDatasourceImpl _solutionDatasource;

    public SolutionService(
        SolutionDatasourceImpl datasourceImpl
        , ILocationService locationService
    )
    {
        _solutionDatasource = datasourceImpl;
        _locationService = locationService;
    }

    public SolutionState GetSolution(
        string path
    )
    {
        var result = _solutionDatasource.ReadSolution(path);

        return result.Match(Succ: succ => succ.ToSolutionState(_locationService)
            , Fail: fail =>
            {
                Logger.LogError(fail);

                return new SolutionState(_locationService);
            });
    }

    public async Task CreateSolution(
        string path
        , SolutionState solution
    )
    {
        var solutionPath = Path.Combine(path1: path
            , path2: $"{solution.FileName}{Constants.AppProjectSolutionExtension}");

        solution.Report.SolutionName = solution.FileName;

        await _solutionDatasource.SaveSolution(solutionPath: solutionPath, dataToWrite: solution.ToSolutionItemRoot());
        _solutionDatasource.CreateFolders(path);
    }

    public async Task SaveSolution(
        string path
        , SolutionState solution
    ) =>
        await _solutionDatasource.SaveSolution(solutionPath: path, dataToWrite: solution.ToSolutionItemRoot());
}

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
            Email = companyInfo.Email
            ,
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
            Logradouro = endereco.Logradouro
            ,
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
            Logradouro = endereco.Logradouro
            ,
            Numero = endereco.Numero
        };

    public static PartnerLogoState ToPartnerLogoState(
        this PartnerItem partner
    ) =>
        new()
        {
            Logo = partner.PartnerLogo,
            Name = partner.NomeEmpresa
        };
}