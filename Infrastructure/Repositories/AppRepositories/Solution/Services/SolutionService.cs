using Application.Solution.Contracts;
using AppRepositories.Solution.Contracts;
using Domain.Solutions;
using Domain.Solutions.ProjectItem;
using Domain.Solutions.Report;
using Domain.Solutions.ValueObjects;

namespace AppRepositories.Solution.Services;

public class SolutionService : ISolutionRepository
{
    private readonly ISolutionDatasource _datasource;

    public SolutionService(
        ISolutionDatasource datasource
    )
    {
        _datasource = datasource;
    }

    public Task<ISolution> OpenSolution(
        string path
    ) => Task.FromResult(_datasource.ReadSolution(path)
        .Match<ISolution>(Succ: s => new Domain.Solutions.Solution
        {
            Report = new Report
            {
                Data = s.Report.Data, Email = s.Report.Email, Endereco = new Endereco
                {
                    Bairro = s.Report.Endereco.Bairro, Cidade = s.Report.Endereco.Cidade
                    , Logradouro = s.Report.Endereco.Logradouro, Numero = s.Report.Endereco.Numero
                    , UF = s.Report.Endereco.UF
                }
                , Responsavel = s.Report.Responsavel, Telefone = s.Report.Telefone, LogoPath = s.Report.LogoPath
                , NomeEmpresa = s.Report.NomeEmpresa, SolutionName = s.Report.SolutionName
            }
            , ProjectItems = s.ProjectItems.Select(it => new ProjectItem
                { Name = it.Name, TemplateName = it.TemplateName, Group = it.Group, Locale = it.Locale })
        }, Fail: e => new Domain.Solutions.Solution()));

    public Task SaveSolution(
        string path
        , ISolution solutionToSave
    ) =>
        throw new NotImplementedException();
}