using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using Common;
using LanguageExt;
using LanguageExt.Common;
using ProjectItemReader.InternalAppFiles.DTO;
using ProjetoAcessibilidade.Core.Entities.App;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Core.Entities.Solution.ItemsGroup;
using ProjetoAcessibilidade.Core.Entities.Solution.ReportInfo;
using ProjetoAcessibilidade.Domain.Solution.Contracts;

namespace ProjectItemReader.InternalAppFiles;

public class SolutionRepositoryImpl : ISolutionRepository
{
    private static XmlSerializer _serializer;

    public async Task SaveSolution(
        Option<string> solutionPath
        , Option<ProjectSolutionModel> dataToWrite
    ) =>
        solutionPath
            .Map(
                path =>
                {
                    var serializer = Option<XmlSerializer>.Some(new XmlSerializer(typeof(SolutionItemRoot)));

                    return serializer.Map(
                        result =>
                        {
                            return dataToWrite.Map(
                                    data =>
                                    {
                                        using var writer =
                                            new StreamWriter(File.Create(path));

                                        result.Serialize(
                                            writer,
                                            data.ToItemRoot());
                                        return true;
                                    })
                                .Match(it => it, () => false);
                        });
                })
            .Match(it => it, () => false);

    public async Task SyncSolution(
        Option<string> solutionPath
        , Option<ProjectSolutionModel> dataToWrite
    ) =>
        solutionPath.Map(
            path =>
            {
                var xml = CreateSolutionStructure(new XmlDocument());

                return dataToWrite.MapAsync(
                        async data =>
                        {
                            var writer = new StreamWriter(
                                path,
                                false);
                            try
                            {
                                xml = SetReportData(
                                    xml,
                                    data.SolutionReportInfo);
                                xml = SetItemsGroup(
                                    xml,
                                    data.ItemGroups.ToList());
                                xml.Save(writer);

                                return true;
                            }
                            finally
                            {
                                writer.Close();
                                await writer.DisposeAsync();
                            }
                        })
                    .Match(b => b, () => false);
            });

    public async Task<Result<ProjectSolutionModel>> ReadSolution(
        Option<string> solutionPath
    ) =>
        await solutionPath
            .MapAsync(
                path => (itemPath: path, exists: File.Exists(path)))
            .Map(item =>
            {
                try
                {
                    if (item.exists)
                    {
                        var serializer = CreateSerializer();

                        using var reader = new StreamReader(item.itemPath);
                        if (serializer.Deserialize(reader) is { } result)
                        {
                            return new Result<SolutionItemRoot>((SolutionItemRoot)result!);
                        }

                        return new Result<SolutionItemRoot>(
                            new Exception($"Erro ao Deserializar{item.itemPath}"));
                    }

                    return new Result<SolutionItemRoot>(
                        new Exception($"Arquivo não existe no caminho {item.itemPath}"));
                }
                catch (Exception ex)
                {
                    return new Result<SolutionItemRoot>(ex);
                }
            })
            .MatchAsync(result => Task.Run(() =>
                {
                    return solutionPath.Match(path =>
                            result.Match(success =>
                                    new Result<ProjectSolutionModel>(success
                                        .ToSolutionInfo(path)),
                                failure => new Result<ProjectSolutionModel>(failure)),
                        () => new Result<ProjectSolutionModel>(new Exception("Impossível de completar leitura")));
                }),
                () => new Result<ProjectSolutionModel>(new Exception("Impossível de completar leitura")));

    private static XmlSerializer CreateSerializer() => _serializer ??= new XmlSerializer(typeof(SolutionItemRoot));

    private SolutionInfo ReadSolutionInfo(
        XmlDocument document
    )
    {
        var elements = document.GetElementsByTagName(Constants.report);

        if (elements.Count > 0)
        {
            var solutionInfo = new SolutionInfo();

            var children = elements[0]
                .ChildNodes
                .Cast<XmlNode>();

            var data = children.First(x => x.Name.Equals(Constants.reportItemData))
                .InnerXml;

            solutionInfo.Data = DateTime.TryParseExact(
                data,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date)
                ? date
                : DateTimeOffset.Now;

            solutionInfo.Responsavel =
                children.First(x => x.Name.Equals(Constants.reportItemResponsavel))
                    .InnerXml;

            solutionInfo.NomeEmpresa =
                children.First(x => x.Name.Equals(Constants.reportItemNomeEmpresa))
                    .InnerXml;

            solutionInfo.LogoPath = children.First(x => x.Name.Equals(Constants.reportItemLogoPath))
                .InnerXml;

            solutionInfo.Telefone = children.First(x => x.Name.Equals(Constants.reportItemTelefone))
                .InnerXml;

            solutionInfo.Endereco = children.First(x => x.Name.Equals(Constants.reportItemEndereco))
                .InnerXml;

            solutionInfo.Email = children.First(x => x.Name.Equals(Constants.reportItemEmail))
                .InnerXml;

            return solutionInfo;
        }

        return default;
    }

    private ItemGroupModel ReadItemGroups(
        XmlNode listItems
        , string solutionPath
    )
    {
        var items = new ItemGroupModel();

        items.Name = listItems?.Attributes
            ?.Cast<XmlAttribute>()
            .First(
                x =>
                    x.Name.Equals(Constants.items_groupsItemGroupAttributeName))
            .Value;

        items.ItemPath = Path.Combine(
            string.Join(
                Path.DirectorySeparatorChar,
                solutionPath.Split(Path.DirectorySeparatorChar)[..^1]),
            Constants.AppProjectItemsFolderName,
            items?.Name);

        foreach (XmlNode item in listItems?.ChildNodes)
        {
            var result = ReadItem(item.ChildNodes.Cast<XmlNode>());

            items.Items.Add(result);
        }

        return items;
    }

    private ItemModel ReadItem(
        IEnumerable<XmlNode> listItems
    )
    {
        var item = new ItemModel();

        item.Id = listItems.First(x => x.Name.Equals(Constants.items_groups_item_id))
            .InnerXml;
        item.Name = listItems.First(x => x.Name.Equals(Constants.items_groups_item_name))
            .InnerXml;
        item.TemplateName = listItems
            .First(x => x.Name.Equals(Constants.items_groups_item_template_name))
            .InnerXml;
        item.ItemPath = listItems.First(x => x.Name.Equals(Constants.items_groups_item_item_path))
            .InnerXml;

        return item;
    }

    private List<ItemGroupModel> ReadProjectItems(
        XmlDocument document
        , string solutionPath
    )
    {
        var elements = document.GetElementsByTagName(Constants.project_items);

        var items = new List<ItemGroupModel>();

        foreach (XmlNode result in elements[0]
                     ?.ChildNodes)
        {
            var itemResult = ReadItemGroups(
                result,
                solutionPath);

            items.Add(itemResult);
        }


        return items;
    }

    private XmlDocument SetItemsGroup(
        XmlDocument xml
        , List<ItemGroupModel> itemGroups
    )
    {
        var elements = xml.GetElementsByTagName(Constants.project_items);
        if (elements.Count > 0 &&
            itemGroups.Count > 0)
        {
            var itemGroupsElement = elements.Item(0);

            foreach (var item in itemGroups)
            {
                var itemGroupItemElement = xml.CreateElement(Constants.items_groups);

                var attribute = xml.CreateAttribute(Constants.items_groupsItemGroupAttributeName);

                attribute.Value = item.Name;
                attribute.InnerXml = item.Name;

                _ = itemGroupItemElement.SetAttributeNode(attribute);

                foreach (var subitem in item.Items)
                {
                    var subItem = xml.CreateElement(Constants.items_groups_item);

                    var subItemId = xml.CreateElement(Constants.items_groups_item_id);
                    subItemId.InnerXml = subitem.Id;

                    var subItemName = xml.CreateElement(Constants.items_groups_item_name);
                    subItemName.InnerXml = subitem.Name;

                    var subItemItemPath = xml.CreateElement(Constants.items_groups_item_item_path);
                    subItemItemPath.InnerXml = subitem.ItemPath;

                    var subItemTemplateName = xml.CreateElement(Constants.items_groups_item_template_name);
                    subItemTemplateName.InnerXml = subitem.TemplateName;

                    _ = subItem.AppendChild(subItemId);
                    _ = subItem.AppendChild(subItemName);
                    _ = subItem.AppendChild(subItemItemPath);
                    _ = subItem.AppendChild(subItemTemplateName);

                    _ = itemGroupItemElement.AppendChild(subItem);
                }

                _ = itemGroupsElement?.AppendChild(itemGroupItemElement);
            }
        }

        return xml;
    }

    private XmlDocument SetReportData(
        XmlDocument xml
        , SolutionInfo reportData
    )
    {
        var elements = xml.GetElementsByTagName(Constants.report);
        if (elements.Count > 0)
        {
            var element = elements.Item(0);

            var email = xml.CreateElement(Constants.reportItemEmail);
            email.InnerText = reportData.Email;

            var endereco = xml.CreateElement(Constants.reportItemEndereco);
            endereco.InnerXml = $"{reportData.Endereco} - {(reportData.UF ??
                                                            new UFModel(
                                                                "",
                                                                "")).Code}";

            var nomeEmpresa = xml.CreateElement(Constants.reportItemNomeEmpresa);
            nomeEmpresa.InnerText = reportData.NomeEmpresa;

            var responsavel = xml.CreateElement(Constants.reportItemResponsavel);
            responsavel.InnerXml = reportData.Responsavel;

            var telefone = xml.CreateElement(Constants.reportItemTelefone);
            telefone.InnerXml = reportData.Telefone;

            var data = xml.CreateElement(Constants.reportItemData);
            data.InnerXml = reportData.Data.ToString("dd/MM/yyyy");

            var logo = xml.CreateElement(Constants.reportItemLogoPath);
            logo.InnerXml = reportData.LogoPath;

            var solutionName = xml.CreateElement(Constants.reportItemSolutionName);
            solutionName.InnerXml = reportData.SolutionName;

            _ = element?.AppendChild(nomeEmpresa);
            _ = element?.AppendChild(responsavel);
            _ = element?.AppendChild(email);
            _ = element?.AppendChild(endereco);
            _ = element?.AppendChild(telefone);
            _ = element?.AppendChild(data);
            _ = element?.AppendChild(logo);
            _ = element?.AppendChild(solutionName);
        }

        return xml;
    }

    private XmlDocument CreateSolutionStructure(
        XmlDocument document
    )
    {
        var root = document.CreateElement(Constants.solutionRoot);
        var report = document.CreateElement(Constants.report);
        var projectItems = document.CreateElement(Constants.project_items);

        _ = root.AppendChild(report);
        _ = root.AppendChild(projectItems);

        _ = document.AppendChild(root);

        return document;
    }

    private string[] GetDirectoryItems(
        string filePath
    )
    {
        if (Directory.Exists(filePath))
        {
            return Directory.GetFiles(filePath);
        }

        return new string[] { };
    }

    private void CreateDirectory(
        string filePath
    )
    {
        if (!Directory.Exists(filePath))
        {
            _ = Directory.CreateDirectory(filePath);
        }
    }
}