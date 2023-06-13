using System.Xml.Serialization;

using Common;

using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;
using ProjetoAcessibilidade.Core.Entities.Solution.ReportInfo;

namespace ProjectItemReader.InternalAppFiles.DTO;

[XmlRoot(elementName: "solution")]
public class SolutionItemRoot
{
    [XmlElement(elementName: "report")]
    public ReportItem Report
    {
        get;
        set;
    }

    [XmlArray(elementName: "project_items")]
    [XmlArrayItem(elementName: "item_groups")]
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
                Data = model.SolutionReportInfo.Data,
                Email = model.SolutionReportInfo.Email,
                Endereco = model.SolutionReportInfo.Endereco,
                LogoPath = model.SolutionReportInfo.LogoPath,
                NomeEmpresa = model.SolutionReportInfo.NomeEmpresa,
                Responsavel = model.SolutionReportInfo.Responsavel,
                SolutionName = model.SolutionReportInfo.SolutionName,
                Telefone = model.SolutionReportInfo.Telefone,
                /*UF = model.SolutionReportInfo.UF.Code*/
            },
            ProjectItems = model.ItemGroups.Select(
                    selector: item => new ProjectItems
                    {
                        ItemName = item.Name,
                        ItemGroup = item.Items.Select(
                                selector: i => new ItemGroup
                                {
                                    Id = i.Id,
                                    ItemPath = i.ItemPath,
                                    Name = i.Name,
                                    TemplateName = i.TemplateName
                                })
                            .ToList()
                    })
                .ToList()
        };

    public static ProjectSolutionModel ToSolutionInfo(
        this SolutionItemRoot model,
string solutionPath
    )
    {
        var r = ProjectSolutionModel.Create(
        solutionPath: solutionPath,
        reportInfo: new SolutionInfo()
        {
            Data = model.Report.Data,
            Email = model.Report.Email,
            Endereco = model.Report.Endereco,
            LogoPath = model.Report.LogoPath,
            NomeEmpresa = model.Report.NomeEmpresa,
            Responsavel = model.Report.Responsavel,
            SolutionName = model.Report.SolutionName,
            Telefone = model.Report.Telefone,
            UF = new(model.Report.UF, "")
        });

        r.FilePath = solutionPath;

        model.ProjectItems.ForEach(item =>
        {
            r.AddItemToSolution(new()
            {
                Name = item.ItemName,
                ItemPath = Path.Combine(Directory.GetParent(solutionPath).FullName, Constants.AppProjectItemsFolderName, item.ItemName),
                Items = item.ItemGroup.Select(x => new ItemModel()
                {
                    Id = x.Id,
                    ItemPath = x.ItemPath,
                    Name = x.Name,
                    TemplateName = x.TemplateName
                })
                    .ToList()
            });

        });


        return r;

    }
}