using System.Xml.Serialization;

namespace XmlDatasource.InternalAppFiles.DTO;

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

/*public static class Extensions
{
    public static SolutionItemRoot ToItemRoot(
        this Domain.Solutions.Solution model
    ) =>
        new()
        {
            Report = new ReportItem()
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

    public static Domain.Solutions.Solution ToSolutionInfo(
        this SolutionItemRoot model
        , string solutionPath
    )
    {
        var r = ProjectSolutionModel.Create(
            solutionPath: solutionPath,
            reportInfo: new SolutionInfo
            {
                Data = model.Report.Data, Email = model.Report.Email, Endereco = model.Report.Endereco
                , LogoPath = model.Report.LogoPath, NomeEmpresa = model.Report.NomeEmpresa
                , Responsavel = model.Report.Responsavel, SolutionName = model.Report.SolutionName
                , Telefone = model.Report.Telefone, UF = new UFModel(code: model.Report.UF, name: "")
            });

        r.FilePath = solutionPath;

        model.ProjectItems.ForEach(item =>
        {
            r.AddItemToSolution(new SolutionGroupModel
            {
                Name = item.ItemName
                , ItemPath = Path.Combine(path1: Directory.GetParent(solutionPath).FullName
                    , path2: Constants.AppProjectItemsFolderName, path3: item.ItemName)
                , Items = item.LocationGroups.Select(x => new ItemGroupModel
                {
                    Name = x.Name
                    , ItemPath = Path.Combine(path1: Directory.GetParent(solutionPath).FullName
                        , path2: Constants.AppProjectItemsFolderName, path3: x.Name)
                    , Items = x.ItemsGroup.Select(it => new ItemModel
                    {
                        Id = it.Id, ItemPath = it.ItemPath, Name = it.Name, TemplateName = it.TemplateName
                    })
                })
            });
        });


        return r;
    }
}*/