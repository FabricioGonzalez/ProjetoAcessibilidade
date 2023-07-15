using System.Xml.Serialization;
using Common;
using ProjetoAcessibilidade.Core.Entities.App;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;
using ProjetoAcessibilidade.Core.Entities.Solution.ReportInfo;

namespace ProjectItemReader.InternalAppFiles.DTO;

[XmlRoot("solution")]
public class SolutionItemRoot
{
    [XmlElement("report")]
    public ReportItem Report
    {
        get;
        set;
    }

    [XmlArray("project_items")]
    [XmlArrayItem("location_item")]
    public List<ProjectItems> ProjectItems
    {
        get;
        set;
    }
}

public static class Extensions
{
    public static SolutionItemRoot ToItemRoot(
        this ProjectSolutionModel model
    ) =>
        new()
        {
            Report = new ReportItem
            {
                Data = model.SolutionReportInfo.Data, Email = model.SolutionReportInfo.Email
                , Endereco = model.SolutionReportInfo.Endereco, LogoPath = model.SolutionReportInfo.LogoPath
                , NomeEmpresa = model.SolutionReportInfo.NomeEmpresa, Responsavel = model.SolutionReportInfo.Responsavel
                , SolutionName = model.SolutionReportInfo.SolutionName, Telefone = model.SolutionReportInfo.Telefone
                /*UF = model.SolutionReportInfo.UF.Code*/
            }
            , ProjectItems = model.LocationItems.Select(it =>
            {
                return new ProjectItems
                {
                    ItemName = it.Name, LocationGroups = it.Items.Select(
                            loc => new LocationGroup
                            {
                                Name = loc.Name, ItemsGroup = loc.Items.Select(
                                        i => new ItemGroup
                                        {
                                            Id = i.Id, ItemPath = i.ItemPath, Name = i.Name
                                            , TemplateName = i.TemplateName
                                        })
                                    .ToList()
                            })
                        .ToList()
                };
            }).ToList()
        };

    public static ProjectSolutionModel ToSolutionInfo(
        this SolutionItemRoot model
        , string solutionPath
    )
    {
        var r = ProjectSolutionModel.Create(
            solutionPath,
            new SolutionInfo
            {
                Data = model.Report.Data, Email = model.Report.Email, Endereco = model.Report.Endereco
                , LogoPath = model.Report.LogoPath, NomeEmpresa = model.Report.NomeEmpresa
                , Responsavel = model.Report.Responsavel, SolutionName = model.Report.SolutionName
                , Telefone = model.Report.Telefone, UF = new UFModel(model.Report.UF, "")
            });

        r.FilePath = solutionPath;

        model.ProjectItems.ForEach(item =>
        {
            r.AddItemToSolution(new SolutionGroupModel
            {
                Name = item.ItemName
                , ItemPath = Path.Combine(Directory.GetParent(solutionPath).FullName
                    , Constants.AppProjectItemsFolderName, item.ItemName)
                , Items = item.LocationGroups.Select(x => new ItemGroupModel
                {
                    Name = x.Name
                    , ItemPath = Path.Combine(Directory.GetParent(solutionPath).FullName
                        , Constants.AppProjectItemsFolderName, x.Name)
                    , Items = x.ItemsGroup.Select(it => new ItemModel
                    {
                        Id = it.Id, ItemPath = it.ItemPath, Name = it.Name, TemplateName = it.TemplateName
                    })
                })
            });
        });


        return r;
    }
}